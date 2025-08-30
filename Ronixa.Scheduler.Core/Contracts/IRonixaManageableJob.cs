namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaManageableJob
    {
        Guid JobId { get; }
        IRonixaJobInformationWithLogging GetStatus();
        bool Start();
        bool Stop();
        bool Pause();
        bool Resume();
    }
}
