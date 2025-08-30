namespace Ronixa.Scheduler.Core.Common
{
    public record JobInfoLastSuccess
        (
            DateTime? At,
            TimeSpan? Duration
        );
}
