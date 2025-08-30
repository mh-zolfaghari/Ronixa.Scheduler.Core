namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobInformationWithLogging : IRonixaJobInformation
    {
        RonixaJobStatus? Status { get; }
        DateTime? LastExecution { get; }
        DateTime? NextExecution { get; }
        JobInfoLastException LastException { get; }
        JobInfoLastSuccess LastSuccess { get; }
    }
}
