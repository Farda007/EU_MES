using EU_MES.Application.Downtime.Commands;
using EU_MES.Domain.Enums;
using EU_MES.Domain.Interfaces;
using Moq;
using Xunit;

namespace EU_MES.Application.Tests;

public class StartDowntimeCommandHandlerTests
{
    private readonly Mock<IDowntimeRepository> _repoMock = new();

    [Fact]
    public async Task Handle_ValidCommand_ShouldCreateDowntime()
    {
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Downtime>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var machineId = Guid.NewGuid();
        var handler = new StartDowntimeCommandHandler(_repoMock.Object);
        var result = await handler.Handle(
            new StartDowntimeCommand(machineId, "Porucha hydrauliky", DowntimeCategory.Breakdown, null, null),
            CancellationToken.None);

        Assert.NotNull(result);
        Assert.Equal(machineId, result.MachineId);
        Assert.Equal("Porucha hydrauliky", result.Reason);
        Assert.Equal(DowntimeCategory.Breakdown, result.Category);
        Assert.Null(result.EndedAt);
    }

    [Fact]
    public async Task Handle_EndDowntime_NotFound_ShouldThrow()
    {
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Downtime?)null);

        var handler = new EndDowntimeCommandHandler(_repoMock.Object);
        await Assert.ThrowsAsync<KeyNotFoundException>(
            () => handler.Handle(new EndDowntimeCommand(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Handle_EndDowntime_ShouldSetEndedAt()
    {
        var machineId = Guid.NewGuid();
        var dt = Domain.Entities.Downtime.Create(machineId, "Test reason", DowntimeCategory.Planned);

        _repoMock.Setup(r => r.GetByIdAsync(dt.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(dt);
        _repoMock.Setup(r => r.UpdateAsync(It.IsAny<Domain.Entities.Downtime>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _repoMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new EndDowntimeCommandHandler(_repoMock.Object);
        var result = await handler.Handle(new EndDowntimeCommand(dt.Id), CancellationToken.None);

        Assert.NotNull(result.EndedAt);
        Assert.NotNull(result.DurationMinutes);
    }
}
