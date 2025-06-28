using Microsoft.AspNetCore.Mvc;
using TaxiNT.Extensions;
using TaxiNT.Libraries.Entities;
using TaxiNT.Services.Interfaces;

namespace TaxiNT.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CheckerDetailController : ControllerBase
{
    //Get API Server
    private readonly IOrderService context;
    private readonly ILogger<CheckerDetailController> logger;
    public CheckerDetailController(IOrderService _context, ILogger<CheckerDetailController> _logger)
    {
        this.context = _context;
        this.logger = _logger;
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetCheckerDetail(string userId)
    {
        try
        {
            //Logic phiếu checker
            var revenue = await context.GetRevenue(userId); //Đã có xữ lý Banking
            var contract = await context.GetContract(userId);
            var timepiece = await context.GetTimepiece(userId);

            return Ok(ConvertToDo.ltvConvertCheckerDetailToDo(new CheckerDetailDto(), revenue, timepiece, contract));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error in GetAll");
            return StatusCode(500, "Internal server error");
        }
    }
}
