using NodaTime;
using Shared.ResultPattern;

namespace Domain.Specifications.Schedule;

internal sealed class TimeZoneIdSpecification
{
    public bool TryGetZone(string? timeZoneId, out DateTimeZone? zone, out Error error)
    {
        zone = null;
        error = Error.None;

        if (string.IsNullOrWhiteSpace(timeZoneId))
        {
            error = new Error(ErrorCode.ValidationError, "TimeZoneId cannot be null or whitespace.");
            return false;
        }

        zone = DateTimeZoneProviders.Tzdb.GetZoneOrNull(timeZoneId);

        if (zone is null)
        {
            error = new Error(ErrorCode.ValidationError, $"Invalid time zone ID: '{timeZoneId}'.");
            return false;
        }

        return true;
    }
}