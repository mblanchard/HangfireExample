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
            BackgroundJob.Enqueue(() => Console.WriteLine("Fire-and-forget"));
            BackgroundJob.Schedule(() => Console.WriteLine("Delayed"), TimeSpan.FromSeconds(30));
            RecurringJob.AddOrUpdate(() => Console.WriteLine("Every Minute"), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => RequestProducer.CreateNewRequests(), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate(() => RequestConsumer.ProcessRequests(), Cron.Minutely);

            app.UseHangfireDashboard();
            app.UseHangfireServer();
        }
    }
}
