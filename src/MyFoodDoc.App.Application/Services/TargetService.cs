using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Target;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class TargetService : ITargetService
    {
        private readonly IApplicationContext _context;

        public TargetService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ICollection<OptimizationAreaDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<OptimizationAreaDto>();

            foreach (var optimizationArea in _context.OptimizationAreas)
            {
                var optimizationAreaDto = new OptimizationAreaDto
                {
                    Key = optimizationArea.Key,
                    Name = optimizationArea.Name,
                    Text = optimizationArea.Text,
                    Targets = new List<TargetDto>()
                };

                foreach (var target in await _context.Targets.Where(x => x.OptimizationAreaId == optimizationArea.Id).ToListAsync())
                {
                    var targetDto = new TargetDto
                    { 
                        Id = target.Id,
                        Type = target.Type.ToString(),
                        Title = target.Title,
                        Text = target.Text
                    };

                    //TODO: use constants or enums
                    if (target.Type == TargetType.Adjustment)
                    {
                        var adjustmentTarget = await _context.AdjustmentTargets.SingleAsync(x => x.TargetId == target.Id);

                        targetDto.Answers = new[] {
                            new TargetAnswerDto { Code = "recommended", Value = string.Format(adjustmentTarget.RecommendedText, adjustmentTarget.TargetValue)},
                            new TargetAnswerDto { Code = "target", Value = adjustmentTarget.TargetText},
                            new TargetAnswerDto { Code = "remain", Value = adjustmentTarget.RemainText }
                        };
                    }
                    else {
                        targetDto.Answers = new[] {
                            new TargetAnswerDto { Code = "yes", Value ="Ja"},
                            new TargetAnswerDto { Code = "no", Value = "Nein" }
                        };
                    }

                    var userAnswer = await _context.UserTargets.SingleOrDefaultAsync(x => x.UserId == userId && x.TargetId == target.Id);

                    if (userAnswer != null)
                        targetDto.UserAnswerCode = userAnswer.TargetAnswerCode;

                    optimizationAreaDto.Targets.Add(targetDto);
                }


                result.Add(optimizationAreaDto);
            }

            return result;
        }
         
        public async Task InsertAsync(string userId, InsertTargetPayload payload, CancellationToken cancellationToken)
        {
            await _context.UserTargets.AddRangeAsync(payload.Targets.Select(x => new UserTarget { UserId = userId, TargetId = x.TargetId, TargetAnswerCode = x.UserAnswerCode }));

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
