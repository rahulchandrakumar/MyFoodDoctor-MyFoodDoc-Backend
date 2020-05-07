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

        public CourseService(IApplicationContext context, IImageBlobService imageService)
        {
            this._context = context;
            this._imageService = imageService;
        }

        public async Task<CourseModel> AddItem(CourseModel item, CancellationToken cancellationToken = default)
        {
            var CourseEntity = item.ToEntity();
            await _context.Courses.AddAsync(CourseEntity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            CourseEntity = await _context.Courses
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(u => u.Id == CourseEntity.Id, cancellationToken);

            return CourseModel.FromEntity(CourseEntity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var CourseEntity = await _context.Courses
                                                .Include(x => x.Image)
                                                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            await _imageService.DeleteImage(CourseEntity.Image.Url, cancellationToken);

            _context.Images.Remove(CourseEntity.Image);
            _context.Courses.Remove(CourseEntity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<CourseModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var CourseEntity = await _context.Courses
                                            .Include(x => x.Image)
                                            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return CourseModel.FromEntity(CourseEntity);
        }

        public async Task<IList<CourseModel>> GetItems(CancellationToken cancellationToken = default)
        {
            var CourseEntities = await _context.Courses
                                                .Include(x => x.Image)
                                                .ToListAsync(cancellationToken);

            return CourseEntities.Select(CourseModel.FromEntity).ToList();
        }

        public IQueryable<Course> GetBaseQuery(string search)
        {
            IQueryable<Course> baseQuery = _context.Courses;
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchstring) || EF.Functions.Like(f.Text, searchstring));
            }
            return baseQuery;
        }

        public async Task<IList<CourseModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var CourseEntities = await GetBaseQuery(search)
                                                .Include(x => x.Image)
                                                .Skip(skip).Take(take)
                                                .ToListAsync(cancellationToken);

            return CourseEntities.Select(CourseModel.FromEntity).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).CountAsync(cancellationToken);
        }

        public async Task<CourseModel> UpdateItem(CourseModel item, CancellationToken cancellationToken = default)
        {
            var CourseEntity = await _context.Courses.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(CourseEntity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(CourseEntity.Id, cancellationToken);
        }
    }
}
