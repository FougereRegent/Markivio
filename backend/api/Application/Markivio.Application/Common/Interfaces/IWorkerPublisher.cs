namespace Markivio.Application.Interfaces;

public interface IWorkerPublisher<in T> where T : class {
	Task SendMessageAsync(T message, CancellationToken token = default);
}
