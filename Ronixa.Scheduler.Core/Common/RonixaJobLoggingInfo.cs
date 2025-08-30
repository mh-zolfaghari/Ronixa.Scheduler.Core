namespace Ronixa.Scheduler.Core.Common
{
    public record RonixaJobLoggingInfo: IRonixaJobStatusInformation
    {
        private Exception? _lastError;
        private DateTime? _lastErrorAt;
        private TimeSpan? _lastErrorDuration;

        private DateTime? _lastDoneAt;
        private TimeSpan? _lastDoneDuration;

        public DateTime StartedAt { get; private set; } = DateTime.UtcNow;
        public TimeSpan? Duration { get; private set; }
        public RonixaJobExecutionState State { get; private set; } = RonixaJobExecutionState.Started;
        public Exception? Exception { get; private set; } = null;
        public DateTime? LastExecution { get; private set; } = null;

        internal void Init()
        {
            StartedAt = DateTime.UtcNow;
            State = RonixaJobExecutionState.Started;
            Duration = null;
            Exception = null;
        }

        internal void ToDone()
        {
            State = RonixaJobExecutionState.Done;
            Duration = DateTime.UtcNow.Subtract(StartedAt);
            LastExecution = DateTime.UtcNow;
            Exception = null;
            _lastDoneDuration = this.Duration.Value;
            _lastDoneAt = DateTime.UtcNow;
        }

        internal void ToError(Exception exception)
        {
            State = RonixaJobExecutionState.Error;
            Duration = DateTime.UtcNow.Subtract(StartedAt);
            LastExecution = DateTime.UtcNow;
            Exception = exception;
            _lastError = exception;
            _lastErrorDuration = this.Duration.Value;
            _lastErrorAt = DateTime.UtcNow;
        }

        public JobInfoLastException GetLastError()
            => new(_lastError, _lastErrorAt, _lastErrorDuration);

        public JobInfoLastSuccess GetLastDone()
            => new(_lastDoneAt, _lastDoneDuration);
    }
}
