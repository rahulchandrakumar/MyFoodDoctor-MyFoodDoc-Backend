using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Method;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.Application.Entites.Abstractions;
using MyFoodDoc.Application.Enums;

namespace MyFoodDoc.App.Application.Services
{
    public class MethodService : IMethodService
    {
        private readonly IApplicationContext _context;
        private readonly ITargetService _targetService;

        public MethodService(IApplicationContext context, ITargetService targetService)
        {
            _context = context;
            _targetService = targetService;
        }

        public async Task<ICollection<MethodDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<MethodDto>();

            var triggered = await _targetService.GetAsync(userId, cancellationToken);

            foreach (var target in triggered.SelectMany(x => x.Targets))
            {
                foreach (var method in await _context.Methods.Where(x => x.TargetId == target.Id)
                    .ToListAsync(cancellationToken))
                {
                    var methodDto = new MethodDto
                    {
                        Id = method.Id,
                        Title = method.Title,
                        Text = method.Text,
                        Type = method.Type.ToString()
                    };

                    if (method.Type == MethodType.YesNo)
                    {
                        var userMethod = await _context.UserMethods.SingleOrDefaultAsync(x => x.UserId == userId && x.MethodId == method.Id, cancellationToken);

                        methodDto.UserAnswer = userMethod?.Answer;
                    }
                    else
                    {
                        methodDto.Choices = new List<MethodMultipleChoiceDto>();

                        var userMethods = await _context.UserMethods
                            .Where(x => x.UserId == userId && x.MethodId == method.Id).Select(x => x.MethodMultipleChoiceId.Value).ToListAsync(cancellationToken);

                        foreach (var methodMultipleChoice in await _context.MethodMultipleChoice
                            .Where(x => x.MethodId == method.Id).ToListAsync(cancellationToken))
                        {
                            methodDto.Choices.Add(new MethodMultipleChoiceDto
                            {
                                Id = methodMultipleChoice.Id,
                                Title = methodMultipleChoice.Title,
                                IsCorrect = methodMultipleChoice.IsCorrect,
                                CheckedByUser = userMethods.Contains(methodMultipleChoice.Id)
                            });
                        }
                    }

                    result.Add(methodDto);
                }
            }

            return result;
        }

        public async Task InsertAsync(string userId, InsertMethodPayload payload, CancellationToken cancellationToken)
        {
            foreach (var item in payload.Methods)
            {
                var method = await _context.Methods.SingleOrDefaultAsync(x => x.Id == item.Id, cancellationToken);

                if (method == null)
                {
                    throw new NotFoundException(nameof(Method), (item.Id));
                }

                if (method.Type == MethodType.YesNo)
                {
                    if (item.UserAnswer != null)
                    {
                        var userMethod = await _context.UserMethods.SingleOrDefaultAsync(x => x.UserId == userId && x.MethodId == method.Id, cancellationToken);

                        if (userMethod == null)
                        {
                            await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, Answer = item.UserAnswer }, cancellationToken);
                        }
                        else
                        {
                            userMethod.Answer = item.UserAnswer;
                            _context.UserMethods.Update(userMethod);
                        }
                    }
                }
                else
                {
                    foreach (var userMethod in await _context.UserMethods.Where(x => x.UserId == userId && x.MethodId == method.Id).ToListAsync(cancellationToken))
                    {
                        _context.UserMethods.Remove(userMethod);
                    }

                    foreach (var choice in item.Choices)
                    {
                        await _context.UserMethods.AddAsync(new UserMethod { UserId = userId, MethodId = method.Id, MethodMultipleChoiceId = choice.Id }, cancellationToken);
                    }
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
