using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.FirebaseClient.Clients;

namespace MyFoodDoc.FirebaseClient.Abstractions
{
    public interface IFirebaseClient
    {
        Task<bool> SendAsync(IEnumerable<FirebaseNotification> notifications, CancellationToken cancellationToken);
    }
}
