using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Method;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IMethodService
    {
        Task<ICollection<MethodDto>> GetAsync(string userId, DateTime date, CancellationToken cancellationToken);
        Task<ICollection<MethodDto>> GetByDateAsync(string userId, DateTime date, CancellationToken cancellationToken);
        Task InsertAsync(string userId, InsertMethodPayload payload, CancellationToken cancellationToken);
    }
}
