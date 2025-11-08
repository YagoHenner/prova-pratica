using Application.Interfaces.Repositories;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LojasHennerDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("LojasHennerDb")));


        services.AddScoped<IUnitOfWork, UnitOfWork>();
        return services;
    }
}