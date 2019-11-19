using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class FoodService : IFoodService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public FoodService(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<IngredientDto>> GetAllAsync(string queryString, CancellationToken cancellationToken)
        {
            var query = _context.Ingredients
                .ProjectTo<IngredientDto>(_mapper.ConfigurationProvider);

            if (!string.IsNullOrEmpty(queryString))
            {
                query = query
                    //.Where(x => CultureInfo.InvariantCulture.CompareInfo.IndexOf(x.Name, queryString, CompareOptions.IgnoreCase) >= 0);
                    .Where(x => x.Name.Contains(queryString));
            }

            return await query.ToListAsync(cancellationToken); ;
        }
    }
}
