namespace Ronixa.Scheduler.Core.Contracts
{
    public interface IRonixaJobInformation
    {
        Guid Id { get; }
        string Title { get; }
        string Description { get; }
    }
}
