using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EU_MES.Infrastructure.Persistence.Seed;

public class DatabaseSeeder
{
    private readonly AppDbContext _ctx;
    private readonly ILogger<DatabaseSeeder> _logger;

    public DatabaseSeeder(AppDbContext ctx, ILogger<DatabaseSeeder> logger)
    {
        _ctx = ctx;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        await _ctx.Database.EnsureCreatedAsync();

        if (await _ctx.Machines.AnyAsync()) return;

        _logger.LogInformation("Seeding database...");

        // Machines
        var machine1 = Machine.Create("CNC-001", "CNC Frézka 1", "CNC obráběcí centrum HAAS VF-2", "Obrobna");
        var machine2 = Machine.Create("WELD-001", "Svářecí robot 1", "Robotické svářecí centrum KUKA", "Svárna");
        var machine3 = Machine.Create("PRESS-001", "Lis hydraulický 1", "Hydraulický lis 200T", "Lisovna");
        _ctx.Machines.AddRange(machine1, machine2, machine3);

        // Workers
        var worker1 = Worker.Create("EMP001", "Jan", "Novák", "Výroba", WorkerShift.Morning);
        var worker2 = Worker.Create("EMP002", "Pavel", "Dvořák", "Výroba", WorkerShift.Morning);
        var worker3 = Worker.Create("EMP003", "Marie", "Horáková", "Kvalita", WorkerShift.Morning);
        var worker4 = Worker.Create("EMP004", "Tomáš", "Procházka", "Výroba", WorkerShift.Afternoon);
        var worker5 = Worker.Create("EMP005", "Lucie", "Nováčková", "Údržba", WorkerShift.Morning);
        _ctx.Workers.AddRange(worker1, worker2, worker3, worker4, worker5);

        // Materials
        var mat1 = Material.Create("MAT-001", "Ocel 1.4301 - plech 2mm", "kg", 500, 100, "Polotovar");
        var mat2 = Material.Create("MAT-002", "Hliník EN AW-6082 - tyč", "kg", 200, 50, "Polotovar");
        var mat3 = Material.Create("MAT-003", "Svářecí drát MIG 0.8mm", "kg", 50, 10, "Spotřební");
        _ctx.Materials.AddRange(mat1, mat2, mat3);

        await _ctx.SaveChangesAsync();

        // Work Orders
        var wo1 = WorkOrder.Create("WO-2024-001", "PROD-A100", "Rámeček hliníkový", 100,
            WorkOrderPriority.High, DateTime.UtcNow.AddDays(-2), DateTime.UtcNow.AddDays(3), machine1.Id);
        wo1.Start();

        var wo2 = WorkOrder.Create("WO-2024-002", "PROD-B200", "Svařenec ocelový", 50,
            WorkOrderPriority.Normal, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(5), machine2.Id);
        wo2.Release();

        var wo3 = WorkOrder.Create("WO-2024-003", "PROD-C300", "Výlisek plastový", 200,
            WorkOrderPriority.Urgent, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), machine3.Id);

        _ctx.WorkOrders.AddRange(wo1, wo2, wo3);
        await _ctx.SaveChangesAsync();

        // Operations for WO1
        var op1 = Operation.Create(wo1.Id, 1, "Frézování vnějšího obrysu", "Frézování dle výkresu K-100-A",
            machine1.Id, 120, worker1.Id);
        op1.Start();

        var op2 = Operation.Create(wo1.Id, 2, "Vrtání otvorů", "Vrtání 8x M6 dle výkresu",
            machine1.Id, 60, worker2.Id);

        var op3 = Operation.Create(wo2.Id, 1, "Svařování rámu", "MIG svařování dle výkresu S-200",
            machine2.Id, 90, worker2.Id);

        _ctx.Operations.AddRange(op1, op2, op3);

        // Quality checks
        var qc1 = QualityCheck.Create(wo1.Id, "Rozměrová kontrola - délka", QualityCheckType.Dimensional,
            op1.Id, 149.5m, 150.5m, worker3.Id);
        qc1.EvaluateResult(150.1m, "V toleranci");

        var qc2 = QualityCheck.Create(wo1.Id, "Vizuální kontrola povrchu", QualityCheckType.Visual,
            null, null, null, worker3.Id);
        qc2.SetResult(QualityCheckResult.Pass, null, "Povrch bez vad");

        _ctx.QualityChecks.AddRange(qc1, qc2);

        // Non-conformances
        var nc1 = NonConformance.Create("NC-202401-0001", NCType.Product, NCSeverity.Minor,
            "Rozměrová odchylka mimo toleranci na operaci frézování",
            wo1.Id, op1.Id, machine1.Id, worker1.Id);

        var nc2 = NonConformance.Create("NC-202401-0002", NCType.Equipment, NCSeverity.Major,
            "Porucha chladícího systému CNC stroje",
            null, null, machine1.Id, null);

        _ctx.NonConformances.AddRange(nc1, nc2);

        // Downtime
        var dt1 = Downtime.Create(machine1.Id, "Výměna nástroje - plánovaná", DowntimeCategory.Planned,
            wo1.Id, worker5.Id);

        _ctx.Downtimes.Add(dt1);

        await _ctx.SaveChangesAsync();
        _logger.LogInformation("Database seeded successfully.");
    }
}
