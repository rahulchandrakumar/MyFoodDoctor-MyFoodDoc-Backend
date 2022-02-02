using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities.Psychogramm;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Application.Persistence.Base;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class QuestionService : IQuestionService
    {
        private readonly IApplicationContext _context;
        private readonly IChoiceService _choiceService;

        public QuestionService(IApplicationContext context, IChoiceService choiceService)
        {
            this._context = context;
            this._choiceService = choiceService;
        }

        public async Task<QuestionModel> AddItem(QuestionModel item, CancellationToken cancellationToken = default)
        {
            var entity = item.ToEntity();
            await _context.Questions.AddAsync(entity, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            entity = await _context.Questions
                .FirstOrDefaultAsync(u => u.Id == entity.Id, cancellationToken);

            return QuestionModel.FromEntity(entity);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Questions
                .Include(x => x.Choices)
                .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

            foreach (var question in entity.Choices.ToList())
                await _choiceService.DeleteItem(question.Id, cancellationToken);

            _context.Questions.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<QuestionModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Questions
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            return QuestionModel.FromEntity(entity);
        }

        public IQueryable<Question> GetBaseQuery(int parentId, string search)
        {
            IQueryable<Question> baseQuery = _context.Questions
                .Where(x => x.ScaleId == parentId);

            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchString = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Text, searchString));
            }
            return baseQuery;
        }

        public async Task<PaginatedItems<QuestionModel>> GetItems(int parentId, int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            var entities = await GetBaseQuery(parentId, search).AsNoTracking().ToListAsync(cancellationToken);

            return new PaginatedItems<QuestionModel>()
            {
                Items = entities.OrderBy(x => x.Order).Skip(skip).Take(take).Select(QuestionModel.FromEntity).ToList(),
                TotalCount = entities.Count
            };
        }

        public async Task<QuestionModel> UpdateItem(QuestionModel item, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Questions.FindAsync(new object[] { item.Id }, cancellationToken);

            _context.Entry(entity).CurrentValues.SetValues(item.ToEntity());

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(entity.Id, cancellationToken);
        }
    }
}
