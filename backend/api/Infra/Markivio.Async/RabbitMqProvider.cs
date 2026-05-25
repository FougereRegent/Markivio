using RabbitMQ.AMQP.Client;
using RabbitMQ.AMQP.Client.Impl;
using Microsoft.Extensions.Configuration;
using Markivio.Infra.Async.Worker;

namespace Markivio.Infra.Async;

public class RabbitMqProvider : IAsyncDisposable
{
	private const string rabbitMqPattern = "amqp://{0}:{1}@{2}:{3}/%2f";
	private IEnvironment _environment = null!;
	private IConnection _connection = null!;
	private readonly IConfiguration _configuration;
	private readonly Dictionary<string, IPublisher> _publishers = new Dictionary<string, IPublisher>();

	public IReadOnlyDictionary<string, IPublisher> Publishers { get => _publishers; }

	public RabbitMqProvider(IConfiguration configuration) {
		_configuration = configuration;
	}

	public async Task InitializeAsync(CancellationToken cancellationToken = default!) {
		var connectionString = string.Format(rabbitMqPattern, 
				_configuration["RABBIT_MQ:USER"],
				_configuration["RABBIT_MQ:PASSWORD"],
				_configuration["RABBIT_MQ:HOST"],
				_configuration["RABBIT_MQ:PORT"]
				);

		var settings = ConnectionSettingsBuilder.Create()
			.Uri(new Uri(connectionString))
			.Build();
		_environment = AmqpEnvironment.Create(settings);
		_connection = await _environment.CreateConnectionAsync(cancellationToken);
		//Queue declaration
		await QueueBuilder(cancellationToken);
	}

    public async ValueTask DisposeAsync()
    {
		foreach(var keyValues in _publishers) {
			var publisher = keyValues.Value;
			if(publisher.State != State.Closed)
				await publisher.CloseAsync();
			publisher.Dispose();
		}
		await _environment.CloseAsync();
		await _connection.CloseAsync();
    }

	private async Task QueueBuilder(CancellationToken cancellationToken = default!) {
		var management = _connection.Management();
		// Declare Queue
		var queueSpec = await management.Queue(ArticleWorker.QueueName)
			.Type(QueueType.QUORUM)
			.DeclareAsync();


		// Declare publisher
		var publisher = await _connection.PublisherBuilder()
			.Queue(queueSpec.Name())
			.BuildAsync();

		_publishers.Add(ArticleWorker.QueueName, publisher);
	}
}
