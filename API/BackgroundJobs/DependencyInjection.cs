using Quartz;

namespace API.BackgroundJobs;

public static class DependencyInjection
{
    public static void AddBackgroundJob(this IServiceCollection services)
    {
        services.AddQuartz(options =>
        {
            var jobKey = JobKey.Create(nameof(PriceListUpdateBackgroundJob));
            options
                  .AddJob<PriceListUpdateBackgroundJob>(jobKey)
                  .AddTrigger(trigger => 
                      trigger
                          .ForJob(jobKey)
                          .WithSimpleSchedule(schedule =>
                              schedule.WithInterval(TimeSpan.FromMinutes(1)).RepeatForever()));
        });

        services.AddQuartzHostedService(options =>
            options.WaitForJobsToComplete = true);
    }
}