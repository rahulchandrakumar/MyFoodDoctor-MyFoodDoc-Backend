using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Exceptions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Report;
using MyFoodDoc.Application.Abstractions;
using MyFoodDoc.Application.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Services
{
    public class ReportService : IReportService
    {
        private readonly IApplicationContext _context;
        private readonly IMapper _mapper;

        public ReportService(IApplicationContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ReportDto> GetLatestReportAsync(string userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
            /*
            var result = await _context.Reports
                .OrderByDescending(report => report.EndDate)
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(Report), "latest");
            }

            return result;
            */
        }

        public async Task<ReportDto> GetReportByIdAsync(string userId, int reportId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            
            /*
            var result = await _context.Reports
                .Where(report => report.Id == reportId)
                .ProjectTo<ReportDto>(_mapper.ConfigurationProvider)
                .SingleOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(Report), reportId);
            }

            return result;
            */
        }

        public async Task InsertReportOptimizationsAsync(string userId, int reportId, ReportOptimizationPayload payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            /*
            var result = await _context.Reports
                .Where(report => report.Id == reportId)
                .Include(report => report.Targets)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(Report), "latest");
            }

            var targetLookup = result.Targets.ToDictionary(x => x.Target.Id, x => x);

            foreach (var s in payload)
            {
                var target = targetLookup[s.Id];

                if (target.ResponseDate != null)
                {
                    throw new BadRequestException("");
                }

                target.ResponseDate = DateTime.UtcNow;
                
                switch (target)
                {
                    case ReportValueTarget valueTarget:
                        valueTarget.Value = s.Value;
                        break;
                }
            };

            _context.SaveChanges();
            */
        }

        public async Task<IEnumerable<ReportMethodDto>> GetReportMethodsByDateAsync(string userId, int reportId, DateTime date, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
            /*
            var result = await _context.Reports
                .Where(report => report.Id == reportId)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(Report), "latest");
            }

            return _mapper.Map<IEnumerable<ReportMethodDto>>(result.Methods.Select(m => m.Method));
            */
        }

        

        public async Task UpsertReportMethodsByDateAsync(string userId, int reportId, DateTime date, ReportMethodsPayload payload, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();

            /*
            var result = await _context.Reports
                .Where(report => report.Id == reportId)
                .Include(report => report.Methods)
                .FirstOrDefaultAsync(cancellationToken);

            if (result == null)
            {
                throw new NotFoundException(nameof(Report), "latest");
            }

            var methodLookup = result.Methods.ToDictionary(x => x.Method.Id, x => x);

            foreach (var s in payload)
            {
                var method = methodLookup[s.Id];

                if (method.ResponseDate != null)
                {
                    throw new BadRequestException("");
                }

                method.ResponseDate = DateTime.UtcNow;

                switch (method)
                {
                    case ReportValueMethod valueMethod:
                        valueMethod.Value = s.Value;
                        break;

                    case ReportChoiceMethod choiceMethod:
                        var choices = new List<ReportChoiceMethodChoice>();
                        foreach (var choice in s.Choices)
                        {
                            choices.Add(new ReportChoiceMethodChoice { ReportId = reportId, MethodId = choiceMethod.MethodId, ChoiceId = choice });
                        }
                        choiceMethod.Choices = choices;
                        break;
                }
            };

            _context.SaveChanges();
            */
        }
    }
}
