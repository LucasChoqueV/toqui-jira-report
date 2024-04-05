using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TJR.Common.Results;
using TJR.JiraReport.Services.Commands;
using TJR.JiraReport.Services.ViewModels;

namespace TJR.JiraReport.Api.Controllers
{
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReportController> _logger;

        public ReportController(
            IMediator mediator,
            ILogger<ReportController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [AllowAnonymous]
        [HttpGet("hc")]
        public string HealthCheck()
        {
            return "Working...";
        }

        [HttpPost("generate/json")]
        public async Task<ValueResult<JiraReportViewModel>> GenerateReportJsonAsync(GenerateReportJsonCommand command)
        {
           var response = await _mediator.Send(command).ConfigureAwait(false);
            return response;
        }
    }
}
