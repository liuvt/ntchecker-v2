using Microsoft.AspNetCore.Components.Authorization;
using TaxiNT.Libraries.Entities;

namespace TaxiNT.Client.Services.Interfaces;

public interface IAuthenService
{
    Task Login(GGSUserLoginDto model);
    Task LogOut();

    //Authen
    Task<AuthenticationState> GetAuthenticationStateAsync();
}
