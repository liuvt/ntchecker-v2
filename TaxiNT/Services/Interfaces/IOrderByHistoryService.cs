using TaxiNT.Libraries.Models.GGSheets;

namespace TaxiNT.Services.Interfaces;
public interface IOrderByHistoryService
{
    Task<Revenue> GetRevenue(string userId, string date);
    Task<Timepiece> GetTimepiece(string userId, string date);
    Task<Contract> GetContract(string userId, string date);

    // Checker
    Task<List<RevenueDetail>> GetsRevenueDetail(string userId);
}
