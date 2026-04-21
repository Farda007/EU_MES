using EU_MES.Application.WorkOrders.Commands;
using EU_MES.Application.DTOs;
using EU_MES.Domain.Entities;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using Moq;
using Xunit;

namespace EU_MES.Application.Tests;

public class CreateWorkOrderCommandHandlerTests
{
    private readonly Mock<IWorkOrderRepository> _repoMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateAndReturnWorkOrder()
    {
        var command = new CreateWorkOrderCommand(
            "WO-TEST-001", "PROD-T", "Test Product", 100,
            WorkOrderPriority.Normal, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), null);

        _repoMock.Setup(r => r.AddAsync(It.IsAny<WorkOrder>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CreateWorkOrderCommandHandler(_repoMock.Object);
        var result = await handler.Handle(command, CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal("WO-TEST-001", result.OrderNumber);
        Assert.Equal(WorkOrderStatus.Created, result.Status);
        _repoMock.Verify(r => r.AddAsync(It.IsAny<WorkOrder>(), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_StartWorkOrder_ShouldChangeStatusToInProgress()
    {
        var wo = WorkOrder.Create("WO-001", "PROD-A", "Product A", 100,
            WorkOrderPriority.High, DateTime.UtcNow, DateTime.UtcNow.AddDays(5));

        _repoMock.Setup(r => r.GetByIdAsync(wo.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(wo);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<WorkOrder>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new StartWorkOrderCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new StartWorkOrderCommand(wo.Id), CancellationToken.None);

        Assert.Equal(WorkOrderStatus.InProgress, result.Status);
        Assert.NotNull(result.ActualStartDate);
    }

    [Fact]
    public async Task Handle_StartWorkOrder_NotFound_ShouldThrow()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((WorkOrder?)null);

        var handler = new StartWorkOrderCommandHandler(_repoMock.Object);
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(new StartWorkOrderCommand(Guid.NewGuid()), CancellationToken.None));
    }
}
