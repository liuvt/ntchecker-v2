using TaxiNT.Libraries.Entities;

namespace TaxiNT.MAUI.Services.Interfaces;

public interface ICheckerDetailService
{
    Task<CheckerDetailDto> GetCheckerDetail(string userId);
}
