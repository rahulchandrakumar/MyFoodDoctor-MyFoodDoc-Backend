﻿using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Course;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Courses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class CourseService : ICourseService
    {
        private readonly IApplicationContext _context;

        public CourseService(IApplicationContext context)
        {
            _context = context;
        }

        public async Task<ICollection<CourseDto>> GetAsync(string userId, CancellationToken cancellationToken)
        {
            var result = new List<CourseDto>();

            foreach(var course in await _context.Courses.Where(x => x.IsActive).Include(x => x.Image).Include(x => x.Chapters).OrderBy(x => x.Order).ToListAsync(cancellationToken))
            {
                result.Add(new CourseDto
                {
                    Id = course.Id,
                    Title = course.Title,
                    Text = course.Text,
                    Order = course.Order,
                    ImageUrl = course.Image.Url,
                    CompletedChaptersCount = (await _context.UserAnswers.Where(x => x.UserId == userId).ToListAsync(cancellationToken)).Count(x => course.Chapters.Any(y => y.Id == x.ChapterId && y.Answer == x.Answer)),
                    ChaptersCount = course.Chapters.Count()
                });
            }

            return result;
        }

        public async Task<CourseDetailsDto> GetDetailsAsync(string userId, int courseId, CancellationToken cancellationToken)
        {
            var course = await _context.Courses.Where(x => x.Id == courseId).Include(x => x.Image).Include(x => x.Chapters).ThenInclude(x => x.Subchapters).SingleAsync();

            var chapters = new List<ChapterDto>();

            foreach (var chapter in course.Chapters.OrderBy(x => x.Order))
            {
                var userAnswer = await _context.UserAnswers.SingleOrDefaultAsync(x => x.UserId == userId && x.ChapterId == chapter.Id, cancellationToken);

                chapters.Add(new ChapterDto
                {
                    Id = chapter.Id,
                    Title = chapter.Title,
                    Text = chapter.Text,
                    Order = chapter.Order,
                    ImageUrl = (await _context.Images.Where(x => x.Id == chapter.ImageId).SingleAsync(cancellationToken)).Url,
                    Subchapters = chapter.Subchapters.Select(x => new SubchapterDto
                    {
                        Title = x.Title,
                        Text = x.Text,
                        Order = x.Order,
                    }).OrderBy(x => x.Order).ToList(),
                    QuestionTitle = chapter.QuestionTitle,
                    QuestionText = chapter.QuestionText,
                    AnswerText1 = chapter.AnswerText1,
                    AnswerText2 = chapter.AnswerText2,
                    Answer = chapter.Answer,
                    UserAnswer = userAnswer?.Answer
                });
            }

            return new CourseDetailsDto
            {
                Title = course.Title,
                Text = course.Text,
                Order = course.Order,
                ImageUrl = course.Image.Url,
                Chapters = chapters
            };
        }

        public async Task InsertAnswerAsync(string userId, int courseId, AnswerPayload payload, CancellationToken cancellationToken)
        {
            var userAnswer = await _context.UserAnswers.SingleOrDefaultAsync(x => x.UserId == userId && x.ChapterId == payload.ChapterId, cancellationToken);

            if (userAnswer == null)
            {
                await _context.UserAnswers.AddAsync(new UserAnswer { UserId = userId, ChapterId = payload.ChapterId, Answer = payload.UserAnswer }, cancellationToken);
            }
            else
            {
                userAnswer.Answer = payload.UserAnswer;
                _context.UserAnswers.Update(userAnswer);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}