using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.User;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using MyFoodDoc.Application.Entities.TrackedValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;

        public UserService(IApplicationContext context, IMapper mapper, UserManager<User> userManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<UserDto> GetUserAsync(string userId, CancellationToken cancellationToken = default)
        {
            var result = await _context.Users
                .Where(x => x.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            //var indications = await GetIndicationsAsync(userId, cancellationToken);
            //var motivations = await GetMotivationsAsync(userId, cancellationToken);
            //var diets = await GetDietsAsync(userId, cancellationToken);

            return result;
        }

        public async Task<UserDto> StoreAnamnesisAsync(string userId, AnamnesisPayload payload, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            user.Gender = payload.Gender;
            user.Height = payload.Height;

            var oldIndications = _context.UserIndications.Where(x => x.UserId.Equals(userId));
            _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Weight > 0)
            {
                user.WeightHistory.Add(new UserWeight()
                {
                    Date = DateTime.UtcNow,
                    Value = payload.Weight
                });
            }

            if (payload.Indications != null)
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync();

                var userIndications = indicationsIds.Select(indicationId => new UserIndication { UserId = userId, IndicationId = indicationId });

                _context.UserIndications.AddRange(userIndications);
            }

            var oldMotivations = _context.UserMotivations.Where(x => x.UserId.Equals(userId));
            _context.UserMotivations.RemoveRange(oldMotivations);
            
            if (payload.Motivations != null)
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync();

                var userMotivations = motivationIds.Select(motivationId => new UserMotivation { UserId = userId, MotivationId = motivationId });

                _context.UserMotivations.AddRange(userMotivations);
            }

            // TODO: Add weight behavior

            await _context.SaveChangesAsync(cancellationToken);

            //var indications = await GetIndicationsAsync(userId, cancellationToken);
            //var motivations = await GetMotivationsAsync(userId, cancellationToken);
            //var diets = await GetDietsAsync(userId, cancellationToken);

            var result = await _context.Users
                .Where(x => x.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return result;

        }

        public async Task<UserDto> UpdateUserAsync(string userId, UpdateUserPayload payload, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            if (payload.Age.HasValue) {
                user.Birthday = DateTime.UtcNow.Date.AddYears(-payload.Age.Value);
            }
            user.Gender = payload.Gender;
            user.Height = payload.Height;
            user.InsuranceId = payload.InsuranceId;
            
            _context.Users.Update(user);

            var oldIndications = _context.UserIndications.Where(x => x.UserId.Equals(userId));
            _context.UserIndications.RemoveRange(oldIndications);

            if (payload.Indications != null)
            {
                var indicationsIds = await _context.Indications
                    .Where(x => payload.Indications.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync();

                var userIndications = indicationsIds.Select(indicationId => new UserIndication { UserId = userId, IndicationId = indicationId });

                _context.UserIndications.AddRange(userIndications);
            }

            var oldMotivations = _context.UserMotivations.Where(x => x.UserId.Equals(userId));
            _context.UserMotivations.RemoveRange(oldMotivations);

            if (payload.Motivations != null)
            {
                var motivationIds = await _context.Motivations
                    .Where(x => payload.Motivations.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync();

                var userMotivations = motivationIds.Select(motivationId => new UserMotivation { UserId = userId, MotivationId = motivationId });

                _context.UserMotivations.AddRange(userMotivations);
            }

            var oldDiets = _context.UserDiets.Where(x => x.UserId.Equals(userId));
            _context.UserDiets.RemoveRange(oldDiets);

            if (payload.Diets != null)
            {
                var dietIds = await _context.Diets
                    .Where(x => payload.Diets.ToArray().Contains(x.Key))
                    .Select(x => x.Id)
                    .ToListAsync();

                var userDiets = dietIds.Select(motivationId => new UserDiet { UserId = userId, DietId = motivationId });

                _context.UserDiets.AddRange(userDiets);
            }

            await _context.SaveChangesAsync(cancellationToken);

            //var indications = await GetIndicationsAsync(userId, cancellationToken);
            //var motivations = await GetMotivationsAsync(userId, cancellationToken);
            //var diets = await GetDietsAsync(userId, cancellationToken);

            var result = await _context.Users
                .Where(x => x.Id == userId)
                .ProjectTo<UserDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            return result;
        }

        private async Task<IList<string>> GetIndicationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userIndication in _context.UserIndications
                join indication in _context.Indications on userIndication.IndicationId equals indication.Id
                where userIndication.UserId == userId
                select indication.Key;

            return await query.ToListAsync(cancellationToken);
        }

        private async Task<IList<string>> GetMotivationsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userMotivation in _context.UserMotivations
                join motivation in _context.Motivations on userMotivation.MotivationId equals motivation.Id
                where userMotivation.UserId == userId
                select motivation.Key;

            return await query.ToListAsync(cancellationToken);
        }

        private async Task<IList<string>> GetDietsAsync(string userId, CancellationToken cancellationToken = default)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var query =
                from userDiet in _context.UserDiets
                join diet in _context.Diets on userDiet.DietId equals diet.Id
                where userDiet.UserId == userId
                select diet.Key;

            return await query.ToListAsync(cancellationToken);
        }

        public async Task ChangePasswordAsync(string userId, string oldPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (!await _userManager.CheckPasswordAsync(user, oldPassword))
            {
                throw new BadRequestException("Old password is wrong");
            }
            await _userManager.RemovePasswordAsync(user);
            await _userManager.AddPasswordAsync(user, newPassword);
        }
        

        public async Task UpdateUserHasSubscription(string userId, bool hasSubscription, CancellationToken cancellationToken = default)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (user == null)
            {
                throw new NotFoundException(nameof(User), userId);
            }

            user.HasSubscription = hasSubscription;
            user.HasSubscriptionUpdated = DateTime.Now;

            _context.Users.Update(user);

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
