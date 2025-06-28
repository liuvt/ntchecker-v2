using TaxiNT.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace TaxiNT.Data;

public partial class taxiNTDBContext : IdentityDbContext<AppUser>
{
    //Get config in appsetting
    private readonly IConfiguration configuration;

    public taxiNTDBContext(DbContextOptions<taxiNTDBContext> options, IConfiguration _configuration) : base(options)
    {
        //Models - Etityties
        configuration = _configuration;
    }


    //Config to connection sql server
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlServer(
                configuration["ConnectionStrings:Hosting"] ??
                    throw new InvalidOperationException("Can't find [ConnectionStrings:Hosting] in appsettings.json!")
            );
        }
    }
}

//Create mirations: dotnet ef migrations add Init -o Data/Migrations
//Create database: dotnet ef database update
//Publish project: dotnet publish -c Release --output ./Publish TaxiNT.csproj