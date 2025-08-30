namespace Ronixa.Scheduler.Core
{
    public abstract class RonixaJob : IHostedService, IDisposable, IRonixaJob
    {
        private System.Timers.Timer? _timer = null;
        private readonly IRonixaJobInformationWithLogging _jobInfo;
        private readonly RonixaJobLoggingInfo _loggingInfo;
        private readonly bool _forceExecuteInStartJob;
        private readonly TimeSpan _duration;
        private readonly object _statusLock = new();
        private RonixaJobStatus _status;

        public event EventHandler<IRonixaJobExecuted> Job_Executed = default!;

        protected RonixaJob(TimeSpan duration, bool forceExecuteInStartJob = false)
        {
            _duration = duration;
            _forceExecuteInStartJob = forceExecuteInStartJob;
            _loggingInfo = new();
            _jobInfo = (IRonixaJobInformationWithLogging)SetJobInfo();
            _status = RonixaJobStatus.Running;
        }

        public abstract IRonixaJobInformation SetJobInfo();
        public abstract Task ExecuteAsync(CancellationToken cancellationToken);

        public virtual async Task StartAsync(CancellationToken cancellationToken)
        {
            if (_forceExecuteInStartJob) await DoWork(cancellationToken);
            await ScheduleJob(cancellationToken);
        }

        public virtual async Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Stop();
            _timer?.Dispose();
            _timer = null;
            _status = RonixaJobStatus.Stopped;
            await Task.CompletedTask;
        }

        public virtual void Dispose() => _timer?.Dispose();

        protected async Task ScheduleJob(CancellationToken cancellationToken)
        {
            if (_status == RonixaJobStatus.Stopped || _status == RonixaJobStatus.Error)
                return;

            if (_status == RonixaJobStatus.Paused)
            {
                _timer?.Stop();
                return;
            }

            var currentDateTime = DateTimeOffset.Now;
            var nextDateTime = currentDateTime.AddTicks(_duration.Ticks);
            TimeSpan delay = nextDateTime - currentDateTime;

            if (delay.TotalSeconds > 1)
            {
                if (delay.TotalMilliseconds <= 0)
                {
                    await ScheduleJob(cancellationToken);
                    return;
                }

                _timer = new System.Timers.Timer(delay.TotalMilliseconds);
                _timer.Elapsed += async (sender, args) =>
                {
                    _timer.Dispose();
                    _timer = null;

                    if (_status == RonixaJobStatus.Stopped || _status == RonixaJobStatus.Paused || _status == RonixaJobStatus.Error || cancellationToken.IsCancellationRequested)
                        return;
                    await DoWork(cancellationToken);

                    if (_status == RonixaJobStatus.Stopped || _status == RonixaJobStatus.Paused || _status == RonixaJobStatus.Error || cancellationToken.IsCancellationRequested)
                        return;
                    await ScheduleJob(cancellationToken);
                };
                _timer.Start();
            }
            await Task.CompletedTask;
        }

        private async Task DoWork(CancellationToken cancellationToken)
        {
            if (_status == RonixaJobStatus.Stopped || _status == RonixaJobStatus.Paused || _status == RonixaJobStatus.Error)
                return;

            try
            {
                _loggingInfo.Init();
                await ExecuteAsync(cancellationToken);
                _loggingInfo.ToDone();
            }
            catch (Exception ex)
            {
                _loggingInfo.ToError(ex);
                //_status = RonixaJobStatus.Error;
            }
            finally { InvokeExecutedJob(); }
        }

        protected IRonixaJobInformation GetJobInfo(IRonixaJob baseJob)
        {
            Type jobType = baseJob.GetType();
            var foundedInfoAtt = jobType.GetCustomAttribute<RonixaJobSchedulerAttribute>(false);
            if (foundedInfoAtt is not null)
            {
                var jobInfo = (RonixaJobSchedulerAttribute)foundedInfoAtt!;
                return new RonixaJobInformation
                    (
                        jobInfo.Id,
                        jobInfo.Title,
                        jobInfo.Description
                    );
            }
            throw new Exception($"Job {jobType.Name} does not have a valid {nameof(RonixaJobSchedulerAttribute)} defined.");
        }

        private void InvokeExecutedJob()
            => Job_Executed?.Invoke(this, new RonixaJobExecuted(_jobInfo!, _loggingInfo));

        public Guid JobId
        {
            get
            {
                if (_jobInfo is null)
                    throw new InvalidOperationException("Job information is not set.");
                return _jobInfo!.Id;
            }
        }

        public IRonixaJobInformationWithLogging GetStatus()
        {
            var lastException = _loggingInfo.GetLastError();
            var lastSuccess = _loggingInfo.GetLastDone();

            var status = new RonixaJobInformation
            (
                _jobInfo.Id,
                _jobInfo.Title,
                _jobInfo.Description,
                _status,
                _loggingInfo.LastExecution,
                DateTime.UtcNow.Add(_duration)
            );

            status.SetLastException(lastException.LastError, lastException.At, lastException.Duration);
            status.SetLastSuccess(lastSuccess.At, lastSuccess.Duration);

            return status;
        }

        public bool Start()
        {
            lock (_statusLock)
            {
                if (_status == RonixaJobStatus.Stopped || _status == RonixaJobStatus.Paused)
                {
                    _status = RonixaJobStatus.Running;
                    _ = StartAsync(CancellationToken.None);
                    return true;
                }
                return false;
            }
        }

        public bool Stop()
        {
            lock (_statusLock)
            {
                if (_status == RonixaJobStatus.Running || _status == RonixaJobStatus.Paused)
                {
                    _status = RonixaJobStatus.Stopped;
                    _ = StopAsync(CancellationToken.None);
                    return true;
                }
                return false;
            }
        }

        public bool Pause()
        {
            lock (_statusLock)
            {
                if (_status == RonixaJobStatus.Running)
                {
                    _status = RonixaJobStatus.Paused;
                    _timer?.Stop();
                    return true;
                }
                return false;
            }
        }

        public bool Resume()
        {
            lock (_statusLock)
            {
                if (_status == RonixaJobStatus.Paused)
                {
                    _status = RonixaJobStatus.Running;
                    _ = ScheduleJob(CancellationToken.None);
                    return true;
                }
                return false;
            }
        }
    }
}
