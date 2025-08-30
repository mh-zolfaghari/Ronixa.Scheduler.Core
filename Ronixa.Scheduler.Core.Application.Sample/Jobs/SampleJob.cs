namespace Ronixa.Scheduler.Core.Application.Sample.Jobs
{
    [RonixaJobScheduler
    (
        id: "EDF401E5-4FB7-4B98-B6B8-68528F617432",
        title: "Sample Job",
        description: "This is Sample Job."
    )
]
    public class SampleJob : RonixaJob
    {
        private readonly ILogger<SampleJob> _logger;

        public SampleJob
            (
                IRonixaJobScheduleConfiguration<SampleJob> config,
                ILogger<SampleJob> Logger
            ) : base(config.Duration, config.ForceExecuteInStartJob)
        {
            _logger = Logger;
            this.Job_Executed += SampleJob_Job_Executed;
        }

        private void SampleJob_Job_Executed(object? sender, IRonixaJobExecuted e)
        {
            if (e.Status.State == RonixaJobExecutionState.Done)
                _logger.LogInformation($"{e.Information.Title} job with {e.Status.Duration?.TotalSeconds ?? 0} second is done.");

            if (e.Status.State == RonixaJobExecutionState.Error)
                _logger.LogCritical($"{e.Information.Title} job with {e.Status.Duration?.TotalSeconds ?? 0} second is failed.", e.Status.Exception);
        }

        public override IRonixaJobInformation SetJobInfo() => GetJobInfo(this);

        public async override Task ExecuteAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("SampleJob is executed.");
            await Task.CompletedTask;
        }
    }
}
