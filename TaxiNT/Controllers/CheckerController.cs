using Microsoft.AspNetCore.Mvc;
using TaxiNT.Services.Interfaces;

namespace TaxiNT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckerController : ControllerBase
{
    //Get API Server
    private readonly IOrderByHistoryService context;
    private readonly ILogger<CheckerController> logger;
    public CheckerController(IOrderByHistoryService _context, ILogger<CheckerController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetsRevenueDetail(string userId)
    {
        try
        {
            var result = await context.GetsRevenueDetail(userId);
            return Ok(result);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetAll");
            return StatusCode(500, "Internal server error");
        }
    }
}
