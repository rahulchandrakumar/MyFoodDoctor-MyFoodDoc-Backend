using System.Collections.Generic;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MyFoodDoc.App.Application.Clients;
using MyFoodDoc.App.Application.Clients.FatSecret;
using MyFoodDoc.App.Application.Exceptions;

namespace MyFoodDoc.App.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly IFatSecretClient _fatSecretClient;

        public FoodService(IApplicationContext context, IMapper mapper, IFatSecretClient fatSecretClient)
        {
            _context = context;
            _mapper = mapper;
            _fatSecretClient = fatSecretClient;
        }

        public async Task<ICollection<IngredientDto>> GetAsync(long foodId, CancellationToken cancellationToken)
        {
            var ingredients = await _context.Ingredients
                .Where(x => x.FoodId == foodId)
                .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider).ToListAsync(cancellationToken);

            return ingredients;
        }
    }
}
