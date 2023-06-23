using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TalentConsulting.TalentSuite.Clients.Common;
using TalentConsulting.TalentSuite.Clients.Common.Interfaces;
using TalentConsulting.TalentSuite.Clients.Core.Infrastructure;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Interceptors;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Persistence.Repository;
using TalentConsulting.TalentSuite.Clients.Infrastructure.Service;

namespace TalentConsulting.TalentSuite.Clients.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        var useDbType = configuration.GetValue<string>("UseDbType");

        switch (useDbType)
        {
            case "UseInMemoryDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("TalentDb"));
                break;

            case "UseSqlServerDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            case "UseSqlLite":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlite(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            case "UsePostgresDatabase":
                services.AddDbContext<ApplicationDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection") ?? String.Empty));
                break;

            default:
                services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseInMemoryDatabase("TalentDb"));
                break;
        }

        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
