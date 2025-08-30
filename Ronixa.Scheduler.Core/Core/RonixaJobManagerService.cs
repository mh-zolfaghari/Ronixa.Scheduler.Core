namespace Ronixa.Scheduler.Core.Core
{
    public class RonixaJobManagerService
        (
            IEnumerable<IRonixaManageableJob> jobs
        ) : IRonixaJobManagerService
    {
        private readonly Dictionary<Guid, IRonixaManageableJob> _jobs = jobs.ToDictionary(j => j.JobId, j => j);

        public IEnumerable<IRonixaJobInformationWithLogging> GetAll()
            => _jobs.Values.Select(j => j.GetStatus());

        public IRonixaJobInformationWithLogging? GetJobStatus(Guid id)
            => _jobs.TryGetValue(id, out var job)
                    ? job.GetStatus()
                    : null;

        public bool StartJob(Guid id)
            => _jobs.TryGetValue(id, out var job)
                    ? job.Start()
                    : false;

        public bool PauseJob(Guid id)
            => _jobs.TryGetValue(id, out var job)
                        ? job.Pause()
                        : false;

        public bool StopJob(Guid id)
            => _jobs.TryGetValue(id, out var job)
                    ? job.Stop()
                    : false;
    }
}
