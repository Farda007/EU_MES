using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EU_MES.Infrastructure.Persistence.Configurations;

public class WorkOrderConfiguration : IEntityTypeConfiguration<WorkOrder>
{
    public void Configure(EntityTypeBuilder<WorkOrder> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.OrderNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.OrderNumber).IsUnique();
        builder.Property(x => x.ProductCode).IsRequired().HasMaxLength(50);
        builder.Property(x => x.ProductName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.PlannedQuantity).HasPrecision(18, 4);
        builder.Property(x => x.ProducedQuantity).HasPrecision(18, 4);
        builder.Property(x => x.ScrapQuantity).HasPrecision(18, 4);
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.Priority).HasConversion<string>();
    }
}

public class OperationConfiguration : IEntityTypeConfiguration<Operation>
{
    public void Configure(EntityTypeBuilder<Operation> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(500);
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.ProducedQuantity).HasPrecision(18, 4);
        builder.Property(x => x.ScrapQuantity).HasPrecision(18, 4);
    }
}

public class MachineConfiguration : IEntityTypeConfiguration<Machine>
{
    public void Configure(EntityTypeBuilder<Machine> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.WorkCenter).HasMaxLength(100);
        builder.Property(x => x.Status).HasConversion<string>();
    }
}

public class WorkerConfiguration : IEntityTypeConfiguration<Worker>
{
    public void Configure(EntityTypeBuilder<Worker> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.EmployeeNumber).IsRequired().HasMaxLength(20);
        builder.HasIndex(x => x.EmployeeNumber).IsUnique();
        builder.Property(x => x.FirstName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.LastName).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Department).HasMaxLength(100);
        builder.Property(x => x.Shift).HasConversion<string>();
        builder.Ignore(x => x.FullName);
    }
}

public class MaterialConfiguration : IEntityTypeConfiguration<Material>
{
    public void Configure(EntityTypeBuilder<Material> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Code).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(200);
        builder.Property(x => x.Unit).HasMaxLength(20);
        builder.Property(x => x.StockQuantity).HasPrecision(18, 4);
        builder.Property(x => x.ReorderPoint).HasPrecision(18, 4);
        builder.Ignore(x => x.IsBelowReorderPoint);
    }
}

public class BatchConfiguration : IEntityTypeConfiguration<Batch>
{
    public void Configure(EntityTypeBuilder<Batch> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.BatchNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.BatchNumber).IsUnique();
        builder.Property(x => x.Quantity).HasPrecision(18, 4);
        builder.Property(x => x.Status).HasConversion<string>();
        builder.Property(x => x.TraceabilityNotes).HasMaxLength(1000);
    }
}

public class NonConformanceConfiguration : IEntityTypeConfiguration<NonConformance>
{
    public void Configure(EntityTypeBuilder<NonConformance> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.NCNumber).IsRequired().HasMaxLength(50);
        builder.HasIndex(x => x.NCNumber).IsUnique();
        builder.Property(x => x.Description).IsRequired().HasMaxLength(1000);
        builder.Property(x => x.RootCause).HasMaxLength(1000);
        builder.Property(x => x.CorrectiveAction).HasMaxLength(1000);
        builder.Property(x => x.Type).HasConversion<string>();
        builder.Property(x => x.Severity).HasConversion<string>();
        builder.Property(x => x.Status).HasConversion<string>();
    }
}

public class DowntimeConfiguration : IEntityTypeConfiguration<Downtime>
{
    public void Configure(EntityTypeBuilder<Downtime> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Reason).IsRequired().HasMaxLength(500);
        builder.Property(x => x.Category).HasConversion<string>();
        builder.Ignore(x => x.DurationMinutes);
    }
}

public class QualityCheckConfiguration : IEntityTypeConfiguration<QualityCheck>
{
    public void Configure(EntityTypeBuilder<QualityCheck> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.CheckName).IsRequired().HasMaxLength(200);
        builder.Property(x => x.CheckType).HasConversion<string>();
        builder.Property(x => x.Result).HasConversion<string>();
        builder.Property(x => x.MeasuredValue).HasPrecision(18, 6);
        builder.Property(x => x.MinValue).HasPrecision(18, 6);
        builder.Property(x => x.MaxValue).HasPrecision(18, 6);
        builder.Property(x => x.Notes).HasMaxLength(500);
    }
}
