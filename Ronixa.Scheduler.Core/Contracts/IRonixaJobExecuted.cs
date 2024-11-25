namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobExecuted
    {
        IRonixaJobInformation Information { get; }
        IRonixaJobStatusInformation Status { get; }
    }
}
