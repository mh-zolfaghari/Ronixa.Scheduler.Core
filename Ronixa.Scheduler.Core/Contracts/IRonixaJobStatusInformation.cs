namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobStatusInformation
    {
        DateTime StartedAt { get; }
        TimeSpan? Duration { get; }
        RonixaJobState State { get; }
        Exception? Exception { get; }
    }
}
