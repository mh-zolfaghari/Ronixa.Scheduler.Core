namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobScheduleConfiguration<T>
    {
        TimeSpan Duration { get; set; }
        bool ForceExecuteInStartJob { get; set; }
    }
}
