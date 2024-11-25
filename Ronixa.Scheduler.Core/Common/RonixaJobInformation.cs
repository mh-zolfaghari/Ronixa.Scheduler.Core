namespace Ronixa.Scheduler.Core.Common
{
    internal record RonixaJobInformation(Guid Id, string Title, string Description) : IRonixaJobInformation;
}
