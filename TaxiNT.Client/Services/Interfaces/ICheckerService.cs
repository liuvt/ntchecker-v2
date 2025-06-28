using TaxiNT.Libraries.Entities;

namespace TaxiNT.Client.Services.Interfaces;

public interface ICheckerService
{
    Task<List<CheckerDto>> GetsRevenueDetail(string userId);
}
