namespace Ronixa.Scheduler.Core.Extensions
{
    public static class RonixaJobInfoExtension
    {
        public static IEnumerable<IRonixaJobInformation> GetRonixaJobsInformation(params Assembly[] assemblies)
        {
            var jobs = assemblies
                .SelectMany(x => x.GetExportedTypes())
                .Where(x => x.IsClass
                        && x.IsPublic
                        && x.GetInterface(nameof(IRonixaJob)) is not null
                        && Attribute.IsDefined(x, typeof(RonixaJobSchedulerAttribute))
                      );

            foreach (var job in jobs)
            {
                var infoAttr = job.GetCustomAttribute<RonixaJobSchedulerAttribute>(false);
                var jobInfo = infoAttr!;
                yield return new RonixaJobInformation(jobInfo.Id, jobInfo.Title, jobInfo.Description);
            }
        }
    }
}
