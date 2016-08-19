using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using Hangfire;
using Hangfire.SqlServer;
using HangfireExample.Core;

[assembly: OwinStartup(typeof(HangfireExample.Api.Startup))]

namespace HangfireExample.Api
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            GlobalConfiguration.Configuration.UseSqlServerStorage("DefaultConnection", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });
            BackgroundJob.Enqueue(() => Console.WriteLine("A fire-and-forget job that runs on app start"));
            BackgroundJob.Schedule(() => Console.WriteLine("A fire-and-forget job that runs once after 30 seconds"), TimeSpan.FromSeconds(30));
            RecurringJob.AddOrUpdate("Minutely",() => Console.WriteLine("Every Minute"), Cron.Minutely);
            RecurringJob.AddOrUpdate("Request Producer",() => RequestProducer.CreateNewRequests(), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate("Request Consumer",() => RequestConsumer.ProcessRequests(), Cron.Minutely);

            RecurringJob.AddOrUpdate("Mock job that fails half the time", () => MockJobs.RunWithTransientErrors(), Cron.Minutely);

            var server = new BackgroundJobServer(GenerateServerOptions());

            app.UseHangfireDashboard();
            app.UseHangfireServer(GenerateServerOptions());
        }

        private BackgroundJobServerOptions GenerateServerOptions()
        {
            return new BackgroundJobServerOptions { ServerName = String.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString()) };
        }
    }
}
