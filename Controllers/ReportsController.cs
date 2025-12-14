using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using cneProyectoVotacion.Services;

namespace cneProyectoVotacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "admin")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("dashboard")]
        public async Task<IActionResult> GetDashboard()
        {
            var stats = await _reportService.GetVoteStatistics();
            return Ok(stats);
        }
    }
}
