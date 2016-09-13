using System;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;
using HangfireExample.Core;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(HangfireExample.Dashboard.Startup))]

namespace HangfireExample.Dashboard
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireExample", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) }); HangfireHelpers.AddOrUpdateRecurringMockTasks();
            app.UseHangfireDashboard();
        }
    }
}
