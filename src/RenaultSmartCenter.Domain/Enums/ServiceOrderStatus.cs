namespace RenaultSmartCenter.Domain.Enums;

public enum ServiceOrderStatus
{
    Created = 1,
    InProgress = 2,
    WaitingForParts = 3,
    QualityCheck = 4,
    Completed = 5,
    Delivered = 6,
    Cancelled = 7
}
