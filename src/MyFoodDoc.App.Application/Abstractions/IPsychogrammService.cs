using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Psychogramm;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IPsychogrammService
    {
        Task<ICollection<ScaleDto>> GetAsync(string userId, CancellationToken cancellationToken);

        Task InsertChoices(string userId, InsertChoicesPayload payload, CancellationToken cancellationToken);

        Task<PsychogrammEvaluationResultDto> GetEvaluationAsync(string userId, CancellationToken cancellationToken);
    }
}
