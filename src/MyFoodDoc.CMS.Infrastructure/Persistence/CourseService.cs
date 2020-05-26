using System;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites.Courses;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.AzureBlob;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class CourseService : ICourseService
    {
        private readonly IApplicationContext _context;
        private readonly IImageBlobService _imageService;
        private readonly IChapterService _chapterService;

        public CourseService(IApplicationContext context, IImageBlobService imageService, IChapterService chapterService)
        {
            this._context = context;
            this._imageService = imageService;
            this._chapterService = chapterService;
        }

        public async Task<CourseModel> AddItem(CourseModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Courses.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Courses
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return CourseModel.FromEntity(entity, GetUsersCount(entity.Id), GetCompletedByUsersCount(entity.Id));
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Courses
                .Include(x => x.Image)
                .Include(x => x.Chapters)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            foreach (var chapter in entity.Chapters.ToList())
                await _chapterService.DeleteItem(chapter.Id, cancellationToken);

            _context.Courses.Remove(entity);

            _context.Images.Remove(entity.Image);

            await _imageService.DeleteImage(entity.Image.Url, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<CourseModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var course = await _context.Courses
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return CourseModel.FromEntity(course, GetUsersCount(course.Id), GetCompletedByUsersCount(course.Id));
        }
        
        public async Task<IList<CourseModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var entities = await _context.Courses
                                                .Include(x => x.Image)
                                                .ToListAsync(cancellationToken);

            return entities.Select(x => CourseModel.FromEntity(x,  GetUsersCount(x.Id), GetCompletedByUsersCount(x.Id))).ToList();
        }

        public IQueryable<Course> GetBaseQuery(string search)
        {
            IQueryable<Course> baseQuery = _context.Courses;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchString) || EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<IList<CourseModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(search)
                                                .Include(x => x.Image)
                                                .Skip(skip).Take(take)
                                                .ToListAsync(cancellationToken);

            return entities.Select(x => CourseModel.FromEntity(x, GetUsersCount(x.Id), GetCompletedByUsersCount(x.Id))).OrderBy(x => x.Order).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).CountAsync(cancellationToken);
        }

        public async Task<CourseModel> UpdateItem(CourseModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Courses.FindAsync(new object[] { item.Id }, cancellationToken);

            var oldImageId = entity.ImageId;

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());
            
            if (item.Image.Id != oldImageId)
            {
                var oldImage = await _context.Images.SingleAsync(x => x.Id == oldImageId);
                _context.Images.Remove(oldImage);

                await _imageService.DeleteImage(oldImage.Url, cancellationToken);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }

        private int GetUsersCount(int courseId)
        {
            var chapters = _context.Chapters.Where(x => x.CourseId == courseId).ToList();

            int result = _context.UserAnswers.ToList().Where(x => chapters.Any(y => y.Id == x.ChapterId)).Select(x => x.UserId)
                .Distinct().Count();

            return result;
        }

        private int GetCompletedByUsersCount(int courseId)
        {
            var chapters = _context.Chapters.Where(x => x.CourseId == courseId).ToList();

            int result = _context.UserAnswers.ToList()
                .Where(x => chapters.Any(y => y.Id == x.ChapterId && y.Answer == x.Answer)).GroupBy(g => g.UserId).Select(x => x.Count()).Count(x => (decimal)x / chapters.Count * 100 >= 80);

            return result;
        }
    }
}
