using System.Net;
using TaxiNT.Client.Services.Interfaces;
using TaxiNT.Libraries.Models.GGSheets;
using System.Net.Http.Json;

namespace TaxiNT.Client.Services;
public class SalaryService : ISalaryService
{
    private readonly HttpClient httpClient;

    //Constructor
    public SalaryService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<Salary> GetSalary(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            return new Salary();

        try
        {
            var response = await httpClient.GetAsync($"api/Salary/{userId}");

            if (response.StatusCode == HttpStatusCode.NoContent)
                return new Salary();

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<Salary>();
                return result ?? new Salary();
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            // TODO: Log exception nếu cần
            throw new ApplicationException("Lỗi khi gọi API lấy lương", ex);
        }
    }
}
