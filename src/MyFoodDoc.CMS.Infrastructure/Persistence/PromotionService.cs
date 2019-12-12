using Microsoft.EntityFrameworkCore;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using MyFoodDoc.CMS.Application.Models;
using MyFoodDoc.CMS.Application.Persistence;
using MyFoodDoc.CMS.Infrastructure.FileProcessors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.CMS.Infrastructure.Persistence
{
    public class PromotionService : IPromotionService
    {
        private IFileService _fileService;
        private IApplicationContext _context;

        public PromotionService(IFileService fileService, IApplicationContext context)
        {
            this._context = context;
            this._fileService = fileService;
        }

        public async Task<PromotionModel> AddItem(PromotionModel item, CancellationToken cancellationToken = default)
        {
            if (item?.TempFileId == null)
                throw new ArgumentException("Field should contain value", "item.TempFileId");

            var file = await _fileService.GetTempFile(item.TempFileId.Value, cancellationToken);

            var promotion = item.ToEntity();
            promotion.Coupons = (await CouponFileProcessor.ReadCouponFile(file.Data)).Select(code => new Coupon() { Code = code }).ToList();
            if (promotion.Coupons.Count == 0)
                throw new ArgumentException("File should contain values", "item.TempFileId");

            await _context.Promotions.AddAsync(promotion, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(promotion.Id, cancellationToken);
        }

        public async Task<bool> DeleteItem(int id, CancellationToken cancellationToken = default)
        {
            var entity = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
            if (entity == null)
                return false;

            _context.Promotions.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);

            return true;
        }

        public async Task<byte[]> GetCouponsFile(int Id, CancellationToken cancellationToken = default)
        {
            var coupons = await _context.Coupons.Where(x => x.PromotionId == Id).ToListAsync(cancellationToken);
            return await CouponFileProcessor.MakeCouponFile(coupons, cancellationToken);
        }

        public async Task<PromotionModel> GetItem(int id, CancellationToken cancellationToken = default)
        {
            var promotionQueryResult = (await (from p in _context.Promotions
                                                where p.Id == id
                                                select new
                                                {
                                                    entity = p,
                                                    CouponCount = p.Coupons.Count(),
                                                    UsedCouponCount = p.Coupons.Where(c => c.RedeemedBy != null).Count()
                                                }).FirstOrDefaultAsync());

            var promotion = PromotionModel.FromEntity(promotionQueryResult.entity);
            promotion.CouponCount = promotionQueryResult.CouponCount;
            promotion.UsedCouponCount = promotionQueryResult.UsedCouponCount;

            return promotion;
        }

        public IQueryable<Promotion> GetBaseQuery(string search)
        {
            IQueryable<Promotion> baseQuery = _context.Promotions.Include(x => x.Insurance);
            if (!string.IsNullOrWhiteSpace(search))
            {
                var searchstring = $"%{search}%";
                baseQuery = baseQuery.Where(f => EF.Functions.Like(f.Title, searchstring) || EF.Functions.Like(f.Insurance.Name, searchstring));
            }
            return baseQuery;
        }

        public async Task<IList<PromotionModel>> GetItems(int take, int skip, string search, CancellationToken cancellationToken = default)
        {
            return (await(from p in GetBaseQuery(search)
                          select new
                          {
                              entity = p,
                              CouponCount = p.Coupons.Count(),
                              UsedCouponCount = p.Coupons.Where(c => c.RedeemedBy != null).Count()
                          }).Take(take).Skip(skip).ToListAsync(cancellationToken))
                   .Select(x =>
                   {
                       var model = PromotionModel.FromEntity(x.entity);
                       model.CouponCount = x.CouponCount;
                       model.UsedCouponCount = x.UsedCouponCount;

                       return model;
                   }).ToList();
        }

        public async Task<long> GetItemsCount(string search, CancellationToken cancellationToken = default)
        {
            return await GetBaseQuery(search).CountAsync(cancellationToken);
        }

        public async Task<PromotionModel> UpdateItem(PromotionModel item, CancellationToken cancellationToken = default)
        {
            var promoEntity = await _context.Promotions.FirstOrDefaultAsync(x => x.Id == item.Id, cancellationToken);

            promoEntity.IsActive = item.IsActive;
            promoEntity.StartDate = item.StartDate;
            promoEntity.EndDate = item.EndDate;
            promoEntity.Title = item.Title;

            await _context.SaveChangesAsync(cancellationToken);

            return await GetItem(promoEntity.Id, cancellationToken);
        }
    }
}
