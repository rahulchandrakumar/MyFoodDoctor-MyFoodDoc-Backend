using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Enums;
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
        private const int EXTRA_QUESTION_ID = 0;

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
                    Questions = scale.Questions.Where(x=> !x.Extra).Select(x => new QuestionDto
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
                    }).OrderBy(x => x.Order).ToList()
                };

                scaleDto.QuestionsCount = scaleDto.Questions.Count();
                scaleDto.CompletedQuestionsCount = scaleDto.Questions
                    .Count(x => x.Choices.Any(y => y.Checked));

                result.Add(scaleDto);
            }

            return result;
        }

        public async Task<PsychogrammEvaluationResultDto> GetEvaluationAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new PsychogrammEvaluationResultDto();

            var userChoices = await _context.UserChoices
                .Include(x => x.Choice)
                .Where(x => x.UserId == userId)
                .ToListAsync(cancellationToken);

            var scales = await _context.Scales
                .Include(x => x.Questions)
                .ThenInclude(x => x.Choices)
                .ToListAsync(cancellationToken);

            var questionsCount = scales.Sum(x => x.Questions.Count(y => !y.Extra));

            if (userChoices.Count < questionsCount)
            {
                result.Status = PsychogrammEvaluationStatus.NotReady.ToString();
            }

            var scorableUserChoices = userChoices.Where(x => x.Choice.Scorable)
                .Select(x => x.ChoiceId)
                .ToList();

            var answeredQuestionsCount = scales.Select(x => new { Scale = x, Count = x.Questions.Count(y => y.Choices.Any(z => scorableUserChoices.Contains(z.Id)))});
            
            var groupped = answeredQuestionsCount.GroupBy(g => g.Count).OrderBy(g => g.Key).Last();

            if (groupped.Count() == 1)
            {
                result.Status = PsychogrammEvaluationStatus.Ready.ToString();

                var scale = groupped.First().Scale;
                result.Evaluation = new EvaluationDto
                {
                    TypeCode = scale.TypeCode,
                    TypeTitle = scale.TypeTitle,
                    TypeText = scale.TypeText,
                    Characterization = scale.Characterization,
                    Reason = scale.Reason,
                    Treatment = scale.Treatment
                };
            }
            else
            {
                result.Status = PsychogrammEvaluationStatus.ExtraQuestion.ToString();

                result.ExtraQuestion = new QuestionDto
                {
                    Id = EXTRA_QUESTION_ID,
                    Type = QuestionType.Radio.ToString(),
                    Text = "Kreuzen Sie an, was auf Ihr Essverhalten am ehesten zutrifft (nur eine Antwort)",
                    Order = 1,
                    VerticalAlignment = true,
                    Choices = groupped.SelectMany(x => x.Scale.Questions.Where(y => y.Extra).Select(z => new ChoiceDto
                    {
                        Id = z.Choices.First(t => t.Scorable).Id,
                        Text = z.Text,
                        Order = x.Scale.Order,
                    })).OrderBy(x => x.Order).ToList()
                };
            }

            return result;
        }

        public async Task InsertChoices(string userId, InsertChoicesPayload payload, CancellationToken cancellationToken)
        {
            if (payload.Questions.Any())
            {
                var userChoices = await _context.UserChoices
                    .Include(x => x.Choice)
                    .Where(x => x.UserId == userId)
                    .ToListAsync(cancellationToken);

                foreach (var question in payload.Questions)
                {
                    if (question.Id == EXTRA_QUESTION_ID)
                    {
                        if (question.Choices.Count() > 1)
                            throw new BadRequestException($"Extra question doesn't support multiple choice");

                        var extraQuestions = await _context.Questions
                            .Where(x => x.Extra)
                            .Select(x => x.Id)
                            .ToListAsync(cancellationToken);

                        if (userChoices.Any(x => extraQuestions.Contains(x.Choice.QuestionId)))
                            _context.UserChoices.RemoveRange(userChoices.Where(x => extraQuestions.Contains(x.Choice.QuestionId)));

                        if (question.Choices.Any())
                            await _context.UserChoices.AddRangeAsync(question.Choices.Select(x => new UserChoice
                            {
                                UserId = userId,
                                ChoiceId = x.Id
                            }).ToList(), cancellationToken);
                    }
                    else
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
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
