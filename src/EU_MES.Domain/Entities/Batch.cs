using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a production batch for traceability.</summary>
public class Batch : BaseEntity
{
    public string BatchNumber { get; private set; } = string.Empty;
    public Guid WorkOrderId { get; private set; }
    public Guid MaterialId { get; private set; }
    public decimal Quantity { get; private set; }
    public DateTime ProducedAt { get; private set; }
    public DateTime? ExpiryDate { get; private set; }
    public BatchStatus Status { get; private set; } = BatchStatus.InProduction;
    public string TraceabilityNotes { get; private set; } = string.Empty;

    private Batch() { }

    public static Batch Create(string batchNumber, Guid workOrderId, Guid materialId,
        decimal quantity, DateTime? expiryDate = null, string traceabilityNotes = "")
    {
        return new Batch
        {
            BatchNumber = batchNumber,
            WorkOrderId = workOrderId,
            MaterialId = materialId,
            Quantity = quantity,
            ProducedAt = DateTime.UtcNow,
            ExpiryDate = expiryDate,
            TraceabilityNotes = traceabilityNotes
        };
    }

    public void Release() { Status = BatchStatus.Released; SetUpdatedAt(); }
    public void Quarantine() { Status = BatchStatus.Quarantine; SetUpdatedAt(); }
    public void Reject() { Status = BatchStatus.Rejected; SetUpdatedAt(); }
    public void UpdateNotes(string notes) { TraceabilityNotes = notes; SetUpdatedAt(); }
}
