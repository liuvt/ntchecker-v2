using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using TaxiNT.Client.Extensions;
using TaxiNT.Client.Services.Interfaces;
using TaxiNT.Libraries.Entities;

namespace TaxiNT.Client.Services;
public class AuthenService : AuthenticationStateProvider, IAuthenService
{
    #region Constructor and Parameter
    private readonly HttpClient httpClient;
    private readonly ILogger<AuthenService> logger;
    //JavaScript
    private readonly IJSRuntime jS;
    //Key localStorage
    private string key = "_taxintToken";
    //Anonymous authentication state
    private AuthenticationState Anonymous =>
        new AuthenticationState(new System.Security.Claims.ClaimsPrincipal(new ClaimsIdentity()));

    //Constructor
    public AuthenService(HttpClient _httpClient, IJSRuntime _jS, ILogger<AuthenService> _logger)
    {
        this.httpClient = _httpClient;
        this.jS = _jS;
        this.logger = _logger;
    }
    #endregion

    /*
        Login
        - Get API Login Controller by httpClientFactory
        - Set Token to LocalStorage
        - Call BuildAuthenticationState(token) to check state login
    */
    public async Task Login(GGSUserLoginDto model)
    {
        try
        {
            var response = await httpClient.PostAsJsonAsync<GGSUserLoginDto>($"api/Auth/Login", model);

            if (response.IsSuccessStatusCode)
            {
                //Lấy token từ API đăng nhập
                var token = await response.Content.ReadAsStringAsync();

                //Lưu token vào localStorage
                await jS.SetFromLocalStorage(key, token);

                //Kiểm tra trạng thái xác thực
                var state = await BuildAuthenticationState(token);
                NotifyAuthenticationStateChanged(Task.FromResult(state));
            }
            else
            {
                var mess = await response.Content.ReadAsStringAsync();
                throw new Exception(mess);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    /*
        Log Out
        - Remove token in localstorage
        - BuildAuthenticationState check state
    */
    public async Task LogOut()
    {
        try
        {
            await jS.RemoveFromLocalStorage(key);

            //Kiểm tra trạng thái sau khi đăng nhập
            httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
        }
        catch (System.Exception ex)
        {

            throw new Exception(ex.Message);
        }
    }


    #region Authentication State

    /*
        Authentication
        - Get token in localStorage by key
        - Check token by ValidationToken(): bool
        - return BuildAuthenticationState(token)
    */
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        // Lấy token từ localStorage qua JSInterop
        var token = await jS.GetFromLocalStorage(key);

        // Validate token
        if (!ValidateToken(token))
        {
            logger.LogWarning("Token không hợp lệ hoặc không tồn tại.");
            return Anonymous;
        }

        // Tạo AuthenticationState từ token
        return await BuildAuthenticationState(token);
    }

    /*
        Build authentication state
        - Check authorization by token
        - Create ParseClaimsFromJwt get claims
        - Get Notify authentication state
        - return authenticationstate
    */
    private async Task<AuthenticationState> BuildAuthenticationState(string localStorageToken)
    {
        //Lấy token từ localstorage vào chuyển đổi token mặt định
        var token = localStorageToken.Replace("\"", "");

        var identity = new ClaimsIdentity(ParseClaimsFromJwt(token), "jwt");
        httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

        var user = new ClaimsPrincipal(identity);
        var state = new AuthenticationState(user);

        /* Lấy dữ liệu chuyển đổi từ Token sang các cập [Key:Value]
        var _user = state.User;
        var ObjectIdentifier = _user.Claims.Where(c => c.Type == "ObjectIdentifier").FirstOrDefault().Value;
        */

        NotifyAuthenticationStateChanged(Task.FromResult(state));

        return state;
    }

    //Chuyển Token thành cặp [Key:Value]
    private IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var payload = jwt.Split('.')[1];
        var jsonBytes = ParseBase64WithoutPadding(payload);
        var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
    }

    //Parse Base64 Without Padding
    private byte[] ParseBase64WithoutPadding(string base64)
    {
        if (string.IsNullOrWhiteSpace(base64))
            return Array.Empty<byte>();

        // JWT sử dụng Base64Url (khác base64 chuẩn)
        base64 = base64.Replace('-', '+').Replace('_', '/');

        switch (base64.Length % 4)
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
            case 1: throw new FormatException("Invalid Base64 string."); // thêm xử lý an toàn
        }

        return Convert.FromBase64String(base64);
    }

    //Validate
    private bool ValidateToken(string token)
    {
        if (string.IsNullOrEmpty(token))
            return false;

        token = token.Replace("\"", "").Trim();

        var handler = new JwtSecurityTokenHandler();
        return handler.CanReadToken(token);
    }
    #endregion
}
