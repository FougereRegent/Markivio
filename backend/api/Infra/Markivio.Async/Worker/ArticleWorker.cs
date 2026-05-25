using Markivio.Application.Interfaces;
using Markivio.Application.Dto;

namespace Markivio.Infra.Async.Worker;

public class ArticleWorker : IWorkerPublisher<ReadableArticleMessage>
{
	internal const string QueueName = "readability-worker";

    public Task SendMessageAsync(ReadableArticleMessage message)
    {
        throw new NotImplementedException();
    }
}
