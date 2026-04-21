using EU_MES.Domain.Common;
using EU_MES.Domain.Enums;

namespace EU_MES.Domain.Entities;

/// <summary>Represents a quality inspection check.</summary>
public class QualityCheck : BaseEntity
{
    public Guid WorkOrderId { get; private set; }
    public Guid? OperationId { get; private set; }
    public string CheckName { get; private set; } = string.Empty;
    public QualityCheckType CheckType { get; private set; }
    public QualityCheckResult Result { get; private set; } = QualityCheckResult.Pending;
    public decimal? MeasuredValue { get; private set; }
    public decimal? MinValue { get; private set; }
    public decimal? MaxValue { get; private set; }
    public Guid? InspectorWorkerId { get; private set; }
    public DateTime CheckedAt { get; private set; }
    public string? Notes { get; private set; }

    private QualityCheck() { }

    public static QualityCheck Create(Guid workOrderId, string checkName, QualityCheckType checkType,
        Guid? operationId = null, decimal? minValue = null, decimal? maxValue = null,
        Guid? inspectorWorkerId = null)
    {
        return new QualityCheck
        {
            WorkOrderId = workOrderId,
            OperationId = operationId,
            CheckName = checkName,
            CheckType = checkType,
            MinValue = minValue,
            MaxValue = maxValue,
            InspectorWorkerId = inspectorWorkerId,
            CheckedAt = DateTime.UtcNow
        };
    }

    public void SetResult(QualityCheckResult result, decimal? measuredValue = null, string? notes = null)
    {
        Result = result;
        MeasuredValue = measuredValue;
        Notes = notes;
        CheckedAt = DateTime.UtcNow;
        SetUpdatedAt();
    }

    /// <summary>Auto-evaluates result based on measured value vs min/max range.</summary>
    public void EvaluateResult(decimal measuredValue, string? notes = null)
    {
        MeasuredValue = measuredValue;
        Notes = notes;
        CheckedAt = DateTime.UtcNow;

        if (MinValue.HasValue && measuredValue < MinValue.Value)
            Result = QualityCheckResult.Fail;
        else if (MaxValue.HasValue && measuredValue > MaxValue.Value)
            Result = QualityCheckResult.Fail;
        else
            Result = QualityCheckResult.Pass;

        SetUpdatedAt();
    }
}
