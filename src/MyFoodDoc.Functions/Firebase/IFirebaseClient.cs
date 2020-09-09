using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.Functions.Firebase
{
    public interface IFirebaseClient
    {
        Task<bool> SendAsync(IEnumerable<FirebaseNotification> notifications, CancellationToken cancellationToken);
    }
}
