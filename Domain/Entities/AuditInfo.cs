using NodaTime;

namespace Domain.Entities;

public record class AuditInfo
{
    public AuditInfo() 
    {
        Instant instant = SystemClock.Instance.GetCurrentInstant();

        CreatedAt = instant;
        UpdatedAt = instant;
    }

    public Instant CreatedAt { get; }
    public Instant? UpdatedAt { get; private set; }

    public void RecordUpdate() => 
        UpdatedAt = SystemClock.Instance.GetCurrentInstant();
}