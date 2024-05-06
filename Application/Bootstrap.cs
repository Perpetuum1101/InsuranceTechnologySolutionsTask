using Application.Service;
using Application.Service.Contracts;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Application;

public static class Bootstrap
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IClaimService, ClaimService>();
        services.AddScoped<ICoverService, CoverService>();

        services.AddAutoMapper(typeof(Bootstrap));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
