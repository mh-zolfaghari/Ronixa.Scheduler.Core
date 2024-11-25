namespace Ronixa.Scheduler.Core.Common
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false)]
    public class RonixaJobSchedulerAttribute : Attribute
    {
        public RonixaJobSchedulerAttribute(string id, string title, string description)
        {
            if (!Guid.TryParse(id, out Guid result)) throw new ArgumentException($"Invalid GUID from {nameof(id)}");

            Id = result;
            Title = title;
            Description = description;
        }

        public Guid Id { get; private set; }
        public string Name => $"{Title}@{Id}";
        public string Title { get; private set; }
        public string Description { get; private set; }
    }
}
