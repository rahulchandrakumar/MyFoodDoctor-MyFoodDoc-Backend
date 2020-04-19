//using MyFoodDoc.Shared.MailSender;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyFoodDoc.App.Application.Abstractions;
using MyFoodDoc.App.Application.Models;
using MyFoodDoc.App.Application.Payloads.Report;
using MyFoodDoc.App.Application.Payloads.User;
using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading;
using System.Threading.Tasks;

namespace MyFoodDoc.App.Api.Controllers
{
    [Authorize]
    public class ReportController : BaseController
    {
        private readonly IReportService _service;
        private readonly ILogger _logger;

        public ReportController(IReportService service, ILogger<ReportController> logger)
        {
            _service = service;
            _logger = logger;
        }

        [HttpGet("latest")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReportDto>> GetLatestReport(CancellationToken cancellationToken = default) 
        {
            var response = await _service.GetLatestReportAsync(GetUserId(), cancellationToken);

            return Ok(response);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ReportDto>> GetReportById([FromRoute] int reportId, CancellationToken cancellationToken = default)
        {
            var response = await _service.GetReportByIdAsync(GetUserId(), reportId, cancellationToken);

            return Ok(response);
        }

        [HttpPost("{id}/optimization")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddReportOptimizations([FromRoute] int reportId, [FromBody] ReportOptimizationPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.InsertReportOptimizationsAsync(GetUserId(), reportId, payload, cancellationToken);

            return NoContent();
        }

        [HttpGet("{id}/methods/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ReportMethodDto>>> GetReportMethodsByDate([FromRoute] int reportId, [FromRoute] DateTime date, CancellationToken cancellationToken = default)
        {
            var response = await _service.GetReportMethodsByDateAsync(GetUserId(), reportId, date, cancellationToken);

            return Ok(response);
        }

        [HttpPut("{id}/methods/{date}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<IEnumerable<ReportMethodDto>>> UpdateReportMethodsByDate([FromRoute] int reportId, [FromRoute] DateTime date, [FromBody] ReportMethodsPayload payload, CancellationToken cancellationToken = default)
        {
            await _service.UpsertReportMethodsByDateAsync(GetUserId(), reportId, date, payload, cancellationToken);

            return NoContent();
        }
    }
}
