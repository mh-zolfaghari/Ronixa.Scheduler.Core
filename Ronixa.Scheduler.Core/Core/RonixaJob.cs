using System.Runtime;

namespace Ronixa.Scheduler.Core
{
    public abstract class RonixaJob : IHostedService, IDisposable, IRonixaJob
    {
        private System.Timers.Timer? _timer = null;
        private readonly IRonixaJobInformation? _jobInfo;
        private readonly RonixaJobLoggingInfo _loggingInfo;

        private readonly bool _forceExecuteInStartJob;
        private readonly TimeSpan _duration;

        public event EventHandler<IRonixaJobExecuted> Job_Executed = default!;

        protected RonixaJob(TimeSpan duration, bool forceExecuteInStartJob = false)
        {
            _duration = duration;
            _forceExecuteInStartJob = forceExecuteInStartJob;
            _loggingInfo = new();
            _jobInfo = SetJobInfo();
        }

        public abstract IRonixaJobInformation? SetJobInfo();
        public abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_forceExecuteInStartJob) await DoWork(cancellationToken);
            await ScheduleJob(cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            await Task.CompletedTask;
        }

        public virtual void Dispose() => _timer?.Dispose();

        protected async Task ScheduleJob(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTimeOffset.Now;
            var nextDateTime = currentDateTime.AddTicks(_duration.Ticks);
            var delay = nextDateTime - currentDateTime;

            if (delay.TotalSeconds > 1)
            {
                if (delay.TotalMilliseconds <= 0) await ScheduleJob(cancellationToken);

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();
                    _timer = null;

                    if (!cancellationToken.IsCancellationRequested) await DoWork(cancellationToken);
                    if (!cancellationToken.IsCancellationRequested) await ScheduleJob(cancellationToken);
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            try
            {
                _loggingInfo.Init();
                await ExecuteAsync(cancellationToken);
                _loggingInfo.ToDone();
            }
            catch (Exception ex)
            {
                _loggingInfo.ToError(ex);
            }
            finally { InvokeExecutedJob(); }
        }

        protected IRonixaJobInformation? GetJobInfo(IRonixaJob baseJob)
        {
            var jobType = baseJob.GetType();
            var foundedInfoAtt = jobType.GetCustomAttribute<RonixaJobSchedulerAttribute>(false);
            if (foundedInfoAtt != null)
            {
                var jobInfo = (RonixaJobSchedulerAttribute)foundedInfoAtt!;
                return new RonixaJobInformation(jobInfo.Id, jobInfo.Title, jobInfo.Description);
            }
            return null;
        }

        private void InvokeExecutedJob()
            => Job_Executed?.Invoke(this, new RonixaJobExecuted(_jobInfo!, _loggingInfo));
    }
}
