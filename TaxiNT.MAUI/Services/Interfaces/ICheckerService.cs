using TaxiNT.Libraries.Entities;

namespace TaxiNT.MAUI.Services.Interfaces;

public interface ICheckerService
{
    Task<List<CheckerDto>> GetsRevenueDetail(string userId);
}
