namespace Ronixa.Scheduler.Core.Common
{
    public record JobInfoLastException
        (
            Exception? LastError,
            DateTime? At,
            TimeSpan? Duration
        );
}
