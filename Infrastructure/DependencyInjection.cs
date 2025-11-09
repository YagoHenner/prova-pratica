using Application.Interfaces.Repositories;
using Application.Interfaces.Services;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Minio;

namespace Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<LojasHennerDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("LojasHennerDb")));

        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.Configure<MinioOptions>(configuration.GetSection("Minio"));

        services.AddSingleton<IMinioClient>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<MinioOptions>>().Value;

            var optionsNaoPreenchidos = string.IsNullOrWhiteSpace(options.Endpoint) ||
                                        string.IsNullOrWhiteSpace(options.AccessKey) ||
                                        string.IsNullOrWhiteSpace(options.SecretKey);
            if (optionsNaoPreenchidos)
                throw new InvalidOperationException("Configurações 'Minio' não detectadas");

            var endpointUri = new Uri(options.Endpoint);

            var clientBuilder = new MinioClient()
                .WithEndpoint(endpointUri.Host, endpointUri.Port);

            return clientBuilder
                .WithCredentials(options.AccessKey, options.SecretKey)
                .Build();
        });

        services.AddScoped<IServicoEnvioArquivosMinio, ServicoEnvioArquivosMinio>();
        return services;
    }
}