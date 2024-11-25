namespace Ronixa.Scheduler.Core.Common
{
    internal record RonixaJobLoggingInfo: IRonixaJobStatusInformation
    {
        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;
        public TimeSpan? Duration { get; private set; }
        public RonixaJobState State { get; private set; } = RonixaJobState.Started;
        public Exception? Exception { get; private set; } = null;

        internal RonixaJobLoggingInfo Init()
        {
            StartedAt = DateTime.UtcNow;
            State = RonixaJobState.Started;
            Duration = null;
            Exception = null;

            return this;
        }

        internal RonixaJobLoggingInfo ToDone()
        {
            State = RonixaJobState.Done;
            Duration = DateTime.UtcNow.Subtract(StartedAt);
            Exception = null;

            return this;
        }

        internal RonixaJobLoggingInfo ToError(Exception exception)
        {
            State = RonixaJobState.Error;
            Exception = exception;
            Duration = null;

            return this;
        }
    }
}
