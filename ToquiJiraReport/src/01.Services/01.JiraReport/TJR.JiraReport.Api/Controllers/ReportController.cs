using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TJR.JiraReport.Api.Controllers
{
    
    [Route("api/report")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        [HttpGet("hc")]
        public string HealthCheck()
        {
            return "Working...";
        }
    }
}
