namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobManagerService
    {
        IEnumerable<IRonixaJobInformationWithLogging> GetAll();
        bool StartJob(Guid id);
        bool StopJob(Guid id);
        bool PauseJob(Guid id);
        IRonixaJobInformationWithLogging? GetJobStatus(Guid id);
    }
}
