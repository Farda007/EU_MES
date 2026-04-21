using EU_MES.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EU_MES.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<WorkOrder> WorkOrders => Set<WorkOrder>();
    public DbSet<Operation> Operations => Set<Operation>();
    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<Worker> Workers => Set<Worker>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<Batch> Batches => Set<Batch>();
    public DbSet<NonConformance> NonConformances => Set<NonConformance>();
    public DbSet<Downtime> Downtimes => Set<Downtime>();
    public DbSet<QualityCheck> QualityChecks => Set<QualityCheck>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
