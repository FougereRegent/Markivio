using RabbitMQ.AMQP.Client;
using RabbitMQ.AMQP.Client.Impl;
using Microsoft.Extensions.Configuration;

namespace Markivio.Infra.Async;

public class RabbitMqProvider : IAsyncDisposable
{
	private const string rabbitMqPattern = "amqp://{0}:{1}@{2}:{3}/%2f";
	private IEnvironment _environment;
	private IConnection _connection;
	private readonly IConfiguration _configuration;

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
		await _environment.CloseAsync();
		await _connection.CloseAsync();
    }

	private async Task QueueBuilder(CancellationToken cancellationToken = default!) {

	}
}
