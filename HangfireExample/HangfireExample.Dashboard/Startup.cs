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
            HangfireHelpers.Init(shouldUpdateJobs: false);
            app.UseHangfireDashboard();
        }
    }
}
