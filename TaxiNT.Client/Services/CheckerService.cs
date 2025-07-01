using System.Net;
using TaxiNT.Client.Services.Interfaces;
using TaxiNT.Libraries.Models.GGSheets;
using TaxiNT.Libraries.Entities;
using System.Net.Http.Json;

using TaxiNT.Libraries.Extensions;

namespace TaxiNT.Client.Services;
public class CheckerService : ICheckerService
{
    private readonly HttpClient httpClient;

    //Constructor
    public CheckerService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<List<CheckerDto>> GetsRevenueDetail(string userId)
    {
        try
        {
            if (string.IsNullOrEmpty(userId))
                return new List<CheckerDto>();

            var response = await httpClient.GetAsync($"api/Checker/{userId}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new List<CheckerDto>();

                var result = await response.Content.ReadFromJsonAsync<List<RevenueDetail>>();

                if (result == null)
                    return new List<CheckerDto>();

                var revenueDetails = result.Select(r => new CheckerDto
                {
                    userId = r.userId,
                    revenueByMonth = r.revenueByMonth.ltvVNDCurrencyToDecimal(),
                    revenueByDate = r.revenueByDate.ltvVNDCurrencyToDecimal(),
                    totalPrice = r.totalPrice.ltvVNDCurrencyToDecimal(),
                    createdAt = r.createdAt
                }).OrderByDescending(e => e.createdAt).ToList();

                // 1 tài 2 xe thì cần tổng lại một số cột quan trọng
                var groupedDetails = revenueDetails
                    .GroupBy(rd => rd.createdAt)
                    .Select(g => new CheckerDto
                    {
                        userId = g.First().userId,
                        revenueByMonth = g.First().revenueByMonth,
                        revenueByDate = g.Sum(rd => rd.revenueByDate),
                        totalPrice = g.Sum(rd => rd.totalPrice),
                        createdAt = g.Key
                    }).ToList();


                return groupedDetails;
            }

            var error = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"API Error: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            // Có thể log ex ở đây
            throw new ApplicationException("Lỗi khi gọi API", ex);
        }
    }

    public async Task<CheckerDetailDto> GetCheckerDetailHistory(string userId, string date)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/Checker/{userId}/History/{date}");

            if (response.IsSuccessStatusCode)
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new CheckerDetailDto();

                var result = await response.Content.ReadFromJsonAsync<CheckerDetailDto>();

                if (result == null)
                    return new CheckerDetailDto();

                return result;
            }

            var error = await response.Content.ReadAsStringAsync();

            throw new HttpRequestException($"API Error: {response.StatusCode} - {error}");
        }
        catch (Exception ex)
        {
            // Có thể log ex ở đây
            throw new HttpRequestException($"Lỗi không load được data tư server --{ex}");
            throw new ApplicationException("Lỗi khi gọi API", ex);
        }
    }
}
