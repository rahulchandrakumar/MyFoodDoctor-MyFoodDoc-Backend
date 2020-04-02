using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Report;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Application.Abstractions
{
    public interface IReportService
    {
        Task<ReportDto> GetLatestReportAsync(string userId, CancellationToken cancellationToken = default);
        Task<ReportDto> GetReportByIdAsync(string v, int reportId, CancellationToken cancellationToken);
        Task InsertReportOptimizationsAsync(string v, int reportId, ReportOptimizationPayload payload, CancellationToken cancellationToken);
        Task<IEnumerable<ReportMethodDto>> GetReportMethodsByDateAsync(string v, int reportId, DateTime date, CancellationToken cancellationToken);
        Task UpsertReportMethodsByDateAsync(string v, int reportId, DateTime date, ReportMethodsPayload payload, CancellationToken cancellationToken);
    }
}
