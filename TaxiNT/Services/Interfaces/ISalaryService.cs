using TaxiNT.Libraries.Models.GGSheets;

namespace TaxiNT.Services.Interfaces;
public interface ISalaryService
{
    Task<Salary> GetSalary(string userId);
}
