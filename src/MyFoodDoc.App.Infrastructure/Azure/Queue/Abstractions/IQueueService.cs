using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Infrastructure.Azure.Queue.Abstractions
{
    public interface IQueueService
    {
        Task AddMessage(string queueName, string message, CancellationToken cancellationToken = default);

        Task AddMessage<T>(string queueName, T message, CancellationToken cancellationToken = default);
    }
}
