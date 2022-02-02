using MyFoodDoc.FirebaseClient.Clients;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.FirebaseClient.Abstractions
{
    public interface IFirebaseClient
    {
        Task<bool> SendAsync(IEnumerable<FirebaseNotification> notifications, CancellationToken cancellationToken);
    }
}
