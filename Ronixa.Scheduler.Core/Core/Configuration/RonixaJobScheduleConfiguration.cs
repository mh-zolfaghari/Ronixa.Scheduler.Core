namespace Ronixa.Scheduler.Core.Configuration
{
    public class RonixaJobScheduleConfiguration<T> : IRonixaJobScheduleConfiguration<T>
    {
        public TimeSpan Duration { get; set; } = TimeSpan.Zero;
        public bool ForceExecuteInStartJob { get; set; } = false;
    }
}