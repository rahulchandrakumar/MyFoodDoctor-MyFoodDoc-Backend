using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Method;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IMethodService
    {
        Task<ICollection<MethodDto>> GetAsync(string userId, CancellationToken cancellationToken);
        Task InsertAsync(string userId, InsertMethodPayload payload, CancellationToken cancellationToken);
    }
}
