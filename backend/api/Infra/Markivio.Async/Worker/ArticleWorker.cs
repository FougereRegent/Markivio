using Markivio.Application.Interfaces;
using Markivio.Application.Dto;
using RabbitMQ.AMQP.Client.Impl;
using Markivio.Infra.Async.Helper;
using System.Text;

namespace Markivio.Infra.Async.Worker;

public class ArticleWorker(RabbitMqProvider rabbitMqProvider) : IWorkerPublisher<ReadableArticleMessage>
{
	internal const string QueueName = "readability-worker";

    public async Task SendMessageAsync(ReadableArticleMessage message, CancellationToken token = default!)
    {
		var publisher = rabbitMqProvider.Publishers[QueueName];
		var jsonPayload = SerializerHelper.Serialize<ReadableArticleMessage>(message);
		var rabbitMqMessage = new AmqpMessage(Encoding.UTF8.GetBytes(jsonPayload));
		var result = await publisher.PublishAsync(rabbitMqMessage, token);
		if(result.Outcome.State != RabbitMQ.AMQP.Client.OutcomeState.Accepted) 
		{

		}
    }
}
