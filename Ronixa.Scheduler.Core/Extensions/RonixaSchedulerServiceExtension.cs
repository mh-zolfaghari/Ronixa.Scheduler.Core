namespace Ronixa.Scheduler.Core.Extensions
{
    public static class RonixaSchedulerServiceExtension
    {
        public static IServiceCollection AddRonixaSchedulingJob<T>(this IServiceCollection services, Action<IRonixaJobScheduleConfiguration<T>> options) where T : RonixaJob
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options), @"Please provide Schedule Configurations.");

            var config = new RonixaJobScheduleConfiguration<T>();
            options.Invoke(config);

            if (config.Duration == TimeSpan.Zero)
                throw new ArgumentNullException(nameof(RonixaJobScheduleConfiguration<T>.Duration), @"Zero timespan is not allowed.");

            services.AddSingleton<IRonixaJobScheduleConfiguration<T>>(config);
            services.AddSingleton<T>();
            services.AddSingleton<IRonixaManageableJob>(provider => provider.GetRequiredService<T>());
            services.AddSingleton<IHostedService>(provider => provider.GetRequiredService<T>());

            return services;
        }

        public static IServiceCollection AddRonixaJobManagerService(this IServiceCollection services)
        {
            services.AddSingleton<IRonixaJobManagerService, RonixaJobManagerService>();
            return services;
        }
    }
}
