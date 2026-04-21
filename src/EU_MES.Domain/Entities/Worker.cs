using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a production worker.</summary>
public class Worker : BaseEntity
{
    public string EmployeeNumber { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public string Department { get; private set; } = string.Empty;
    public WorkerShift Shift { get; private set; } = WorkerShift.Morning;
    public bool IsActive { get; private set; } = true;

    public string FullName => $"{FirstName} {LastName}";

    private Worker() { }

    public static Worker Create(string employeeNumber, string firstName, string lastName,
        string department, WorkerShift shift)
    {
        return new Worker
        {
            EmployeeNumber = employeeNumber,
            FirstName = firstName,
            LastName = lastName,
            Department = department,
            Shift = shift
        };
    }

    public void Update(string firstName, string lastName, string department, WorkerShift shift)
    {
        FirstName = firstName;
        LastName = lastName;
        Department = department;
        Shift = shift;
        SetUpdatedAt();
    }

    public void Deactivate() { IsActive = false; SetUpdatedAt(); }
    public void Activate() { IsActive = true; SetUpdatedAt(); }
}
