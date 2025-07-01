using System.Net;
using TaxiNT.Client.Services.Interfaces;
using TaxiNT.Libraries.Entities;
using System.Net.Http.Json;

namespace TaxiNT.Client.Services;
public class CheckerDetailService : ICheckerDetailService
{
    private readonly HttpClient httpClient;

    //Constructor
    public CheckerDetailService(HttpClient _httpClient)
    {
        this.httpClient = _httpClient;
    }

    public async Task<CheckerDetailDto> GetCheckerDetail(string userId)
    {
        try
        {
            var response = await httpClient.GetAsync($"api/CheckerDetail/{userId}");

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
        }
    }
}
