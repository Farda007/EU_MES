using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a production machine.</summary>
public class Machine : BaseEntity
{
    public string Code { get; private set; } = string.Empty;
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public string WorkCenter { get; private set; } = string.Empty;
    public MachineStatus Status { get; private set; } = MachineStatus.Available;
    public bool IsActive { get; private set; } = true;

    private Machine() { }

    public static Machine Create(string code, string name, string description, string workCenter)
    {
        return new Machine
        {
            Code = code,
            Name = name,
            Description = description,
            WorkCenter = workCenter
        };
    }

    public void UpdateStatus(MachineStatus status)
    {
        Status = status;
        SetUpdatedAt();
    }

    public void Update(string name, string description, string workCenter)
    {
        Name = name;
        Description = description;
        WorkCenter = workCenter;
        SetUpdatedAt();
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(); }
    public void Activate() { IsActive = true; SetUpdatedAt(); }
}
