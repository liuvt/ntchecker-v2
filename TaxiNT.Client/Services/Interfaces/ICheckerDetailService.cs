using TaxiNT.Libraries.Entities;

namespace TaxiNT.Client.Services.Interfaces;

public interface ICheckerDetailService
{
    Task<CheckerDetailDto> GetCheckerDetail(string userId);
}
