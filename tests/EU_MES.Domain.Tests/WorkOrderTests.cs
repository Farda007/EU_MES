using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using Xunit;

namespace EU_MES.Domain.Tests;

public class WorkOrderTests
{
    private static WorkOrder CreateSampleWorkOrder(WorkOrderStatus? initialStatus = null)
    {
        var wo = WorkOrder.Create("WO-001", "PROD-A", "Test Product", 100,
            WorkOrderPriority.Normal, DateTime.UtcNow, DateTime.UtcNow.AddDays(7));

        if (initialStatus == WorkOrderStatus.Released) wo.Release();
        if (initialStatus == WorkOrderStatus.InProgress) { wo.Release(); wo.Start(); }

        return wo;
    }

    [Fact]
    public void Create_WorkOrder_ShouldHaveCreatedStatus()
    {
        var wo = CreateSampleWorkOrder();
        Assert.Equal(WorkOrderStatus.Created, wo.Status);
    }

    [Fact]
    public void Start_FromCreated_ShouldSetInProgressAndActualStartDate()
    {
        var wo = CreateSampleWorkOrder();
        wo.Start();
        Assert.Equal(WorkOrderStatus.InProgress, wo.Status);
        Assert.NotNull(wo.ActualStartDate);
    }

    [Fact]
    public void Start_FromReleased_ShouldSucceed()
    {
        var wo = CreateSampleWorkOrder(WorkOrderStatus.Released);
        wo.Start();
        Assert.Equal(WorkOrderStatus.InProgress, wo.Status);
    }

    [Fact]
    public void Start_FromCompleted_ShouldThrow()
    {
        var wo = CreateSampleWorkOrder(WorkOrderStatus.InProgress);
        wo.Complete();
        Assert.Throws<InvalidOperationException>(() => wo.Start());
    }

    [Fact]
    public void Complete_FromInProgress_ShouldSetCompletedAndActualEndDate()
    {
        var wo = CreateSampleWorkOrder(WorkOrderStatus.InProgress);
        wo.Complete();
        Assert.Equal(WorkOrderStatus.Completed, wo.Status);
        Assert.NotNull(wo.ActualEndDate);
    }

    [Fact]
    public void Complete_FromCreated_ShouldThrow()
    {
        var wo = CreateSampleWorkOrder();
        Assert.Throws<InvalidOperationException>(() => wo.Complete());
    }

    [Fact]
    public void Cancel_CompletedWorkOrder_ShouldThrow()
    {
        var wo = CreateSampleWorkOrder(WorkOrderStatus.InProgress);
        wo.Complete();
        Assert.Throws<InvalidOperationException>(() => wo.Cancel());
    }

    [Fact]
    public void UpdateProducedQuantity_NegativeValue_ShouldThrow()
    {
        var wo = CreateSampleWorkOrder();
        Assert.Throws<ArgumentException>(() => wo.UpdateProducedQuantity(-10));
    }

    [Fact]
    public void Release_FromCreated_ShouldSucceed()
    {
        var wo = CreateSampleWorkOrder();
        wo.Release();
        Assert.Equal(WorkOrderStatus.Released, wo.Status);
    }

    [Fact]
    public void Release_FromInProgress_ShouldThrow()
    {
        var wo = CreateSampleWorkOrder(WorkOrderStatus.InProgress);
        Assert.Throws<InvalidOperationException>(() => wo.Release());
    }
}
