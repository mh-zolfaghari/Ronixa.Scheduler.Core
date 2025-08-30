namespace Ronixa.Scheduler.Core.Common
{
    internal record RonixaJobInformation
        (
            Guid Id,
            string Title,
            string Description,
            RonixaJobStatus? Status = default,
            DateTime? LastExecution = default,
            DateTime? NextExecution = default
        ) : IRonixaJobInformationWithLogging
    {
        public JobInfoLastException LastException { get; private set; } = new(null, null, null);
        public JobInfoLastSuccess LastSuccess { get; private set; } = new(null, null);


        internal void SetLastException(Exception? lastError, DateTime? at, TimeSpan? duration)
        {
            LastException = LastException with { LastError = lastError, At = at, Duration = duration };
        }

        internal void SetLastSuccess(DateTime? at, TimeSpan? duration)
        {
            LastSuccess = LastSuccess with { At = at, Duration = duration };
        }
    }
}
