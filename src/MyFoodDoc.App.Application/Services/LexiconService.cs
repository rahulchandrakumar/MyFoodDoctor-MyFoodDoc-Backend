using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class LexiconService : ILexiconService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public LexiconService(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ICollection<LexiconShallowEntryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var result = await _context.LexiconEntries

                .ProjectTo<LexiconShallowEntryDto>(_mapper.ConfigurationProvider)
                .OrderBy(e => e.Title)
                .ToListAsync(cancellationToken);

            return result;
        }

        public async Task<LexiconEntryDto> GetAsync(int entryId, CancellationToken cancellationToken = default)
        {
            var result = await _context.LexiconEntries
                .Where(p => p.Id == entryId)
                .ProjectTo<LexiconEntryDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(LexiconEntry), entryId);
            }

            return result;
        }
    }
}
