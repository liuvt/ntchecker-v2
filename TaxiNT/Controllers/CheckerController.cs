using Microsoft.AspNetCore.Mvc;
using TaxiNT.Extensions;
using TaxiNT.Libraries.Entities;
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
            logger.LogError(ex, "Error in GetsRevenueDetail");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{userId}/History/{date}")]
    public async Task<ActionResult<CheckerDetailDto>> GetCheckerDetailHistory(string userId, string date)
    {
        try
        {
            //Logic phiếu checker
            var revenue = await context.GetRevenue(userId,date); //Đã có xữ lý Banking
            var contract = await context.GetContract(userId, date);
            var timepiece = await context.GetTimepiece(userId, date);

            return Ok(ConvertToDo.ltvConvertCheckerDetailToDo(new CheckerDetailDto(), revenue, timepiece, contract));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetCheckerDetailHistory");
            return new CheckerDetailDto();
        }
    }
}
