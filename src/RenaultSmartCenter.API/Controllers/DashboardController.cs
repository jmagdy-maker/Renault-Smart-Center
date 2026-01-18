using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RenaultSmartCenter.Application.Features.Dashboard;

namespace RenaultSmartCenter.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("{branchId}")]
    public async Task<ActionResult<DashboardDto>> GetDashboard(Guid branchId)
    {
        var dashboard = await _dashboardService.GetDashboardDataAsync(branchId);
        return Ok(dashboard);
    }
}
