using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Psychogramm;

namespace MyFoodDoc.App.Application.Services
{
    public class PsychogrammService : IPsychogrammService
    {
        public async Task<ICollection<ScaleDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task InsertChoices(string userId, int scaleId, InsertChoicesPayload payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
