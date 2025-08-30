namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobStatusInformation
    {
        DateTime StartedAt { get; }
        TimeSpan? Duration { get; }
        RonixaJobExecutionState State { get; }
        Exception? Exception { get; }
        DateTime? LastExecution { get; }

        JobInfoLastException GetLastError();
        JobInfoLastSuccess GetLastDone();
    }
}
