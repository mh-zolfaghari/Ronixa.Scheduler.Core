namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJob : IRonixaManageableJob, IHostedService, IDisposable
    {
    }
}
