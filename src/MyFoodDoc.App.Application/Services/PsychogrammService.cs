using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Psychogramm;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Services
{
    public class PsychogrammService : IPsychogrammService
    {
        private readonly IApplicationContext _context;

        public PsychogrammService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ICollection<ScaleDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<ScaleDto>();

            var userChoices = await _context.UserChoices
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            foreach (var scale in await _context.Scales
                .Include(x => x.Image)
                .Include(x => x.Questions)
                .ThenInclude(x => x.Choices)
                .OrderBy(x => x.Order)
                .ToListAsync(cancellationToken))
            {
                var scaleDto = new ScaleDto
                {
                    Id = scale.Id,
                    Title = scale.Title,
                    Text = scale.Text,
                    Order = scale.Order,
                    ImageUrl = scale.Image.Url,
                    Questions = scale.Questions.Select(x => new QuestionDto
                    {
                        Id = x.Id,
                        Type = x.Type.ToString(),
                        Text = x.Text,
                        Order = x.Order,
                        VerticalAlignment = x.VerticalAlignment,
                        Choices = x.Choices.Select(y => new ChoiceDto
                            {
                                Id = y.Id,
                                Text = y.Text,
                                Order = y.Order,
                                Checked = userChoices.Any(z => z.ChoiceId == y.Id)
                        })
                            .OrderBy(x => x.Order).ToList()
                    }).OrderBy(x => x.Order).ToList(),
                    QuestionsCount = scale.Questions.Count
                };

                scaleDto.CompletedQuestionsCount = scaleDto.Questions
                    .Count(x => x.Choices.Any(y => y.Checked));

                result.Add(scaleDto);
            }

            return result;
        }

        public async Task InsertChoices(string userId, int scaleId, InsertChoicesPayload payload, CancellationToken cancellationToken)
        {
            if (payload.Questions.Any())
            {
                var userChoices = await _context.UserChoices
                    .Include(x => x.Choice)
                    .Where(x => x.UserId == userId)
                    .ToListAsync(cancellationToken);

                foreach (var question in payload.Questions)
                {
                    var questionEntity =
                        await _context.Questions.FirstOrDefaultAsync(x => x.Id == question.Id, cancellationToken);

                    if (questionEntity == null)
                        throw new NotFoundException(nameof(Question), question.Id);

                    if (questionEntity.Type != QuestionType.Multiple && question.Choices.Count() > 1)
                        throw new BadRequestException($"Question with id={question.Id} doesn't support multiple choice");

                    if (userChoices.Any(x => x.Choice.QuestionId == question.Id))
                        _context.UserChoices.RemoveRange(userChoices.Where(x => x.Choice.QuestionId == question.Id));

                    if (question.Choices.Any())
                        await _context.UserChoices.AddRangeAsync(question.Choices.Select(x => new UserChoice
                        {
                            UserId = userId,
                            ChoiceId = x.Id
                        }).ToList(), cancellationToken);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
