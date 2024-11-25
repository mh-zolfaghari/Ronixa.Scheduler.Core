namespace Ronixa.Scheduler.Core.Common
{
    internal record RonixaJobExecuted(IRonixaJobInformation Information, IRonixaJobStatusInformation Status) : IRonixaJobExecuted;
}
