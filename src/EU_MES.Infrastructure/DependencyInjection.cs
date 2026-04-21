using EU_MES.Domain.Interfaces;
using EU_MES.Infrastructure.Persistence;
using EU_MES.Infrastructure.Persistence.Repositories;
using EU_MES.Infrastructure.Persistence.Seed;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EU_MES.Infrastructure;

public static class DependencyInjection
{
    /// <summary>Registers Infrastructure layer services.</summary>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite(configuration.GetConnectionString("DefaultConnection")
                ?? "Data Source=EU_MES.db"));

        services.AddScoped<IWorkOrderRepository, WorkOrderRepository>();
        services.AddScoped<IOperationRepository, OperationRepository>();
        services.AddScoped<IMachineRepository, MachineRepository>();
        services.AddScoped<IWorkerRepository, WorkerRepository>();
        services.AddScoped<IMaterialRepository, MaterialRepository>();
        services.AddScoped<IBatchRepository, BatchRepository>();
        services.AddScoped<IDowntimeRepository, DowntimeRepository>();
        services.AddScoped<INonConformanceRepository, NonConformanceRepository>();
        services.AddScoped<IQualityCheckRepository, QualityCheckRepository>();
        services.AddScoped<DatabaseSeeder>();

        return services;
    }
}
