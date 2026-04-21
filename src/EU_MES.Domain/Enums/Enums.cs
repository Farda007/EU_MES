namespace EU_MES.Domain.Enums;

public enum WorkOrderStatus { Created, Released, InProgress, Completed, Cancelled }
public enum WorkOrderPriority { Low, Normal, High, Urgent }
public enum OperationStatus { Pending, InProgress, Completed, Paused, Cancelled }
public enum MachineStatus { Available, Running, Stopped, Maintenance, Breakdown }
public enum WorkerShift { Morning, Afternoon, Night }
public enum BatchStatus { InProduction, Released, Quarantine, Rejected }
public enum NCType { Process, Product, Material, Equipment }
public enum NCSeverity { Minor, Major, Critical }
public enum NCStatus { Open, UnderReview, ActionRequired, Closed }
public enum DowntimeCategory { Planned, Unplanned, Breakdown, Setup, Changeover, NoPlan }
public enum QualityCheckType { Visual, Dimensional, Functional, Chemical }
public enum QualityCheckResult { Pass, Fail, Pending }
