using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;

namespace HangfireExample.Core
{
    public static class HangfireHelpers
    {
        public static  BackgroundJobServerOptions GenerateServerOptions()
        {
            return new BackgroundJobServerOptions
            {
                ServerName = String.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString()),
                WorkerCount = 4 //Low WorkerCount to force distributed task load
            }; 
        }

        public static void Init(bool shouldUpdateJobs)
        {
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireExample", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });
            if(shouldUpdateJobs) HangfireHelpers.AddOrUpdateRecurringMockTasks();
        }

        public static void AddOrUpdateRecurringMockTasks()
        {
            RecurringJob.AddOrUpdate(() => MockTasks.SlowTask(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => MockTasks.SlowerTask(), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate(() => MockTasks.SlowestTask(), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => MockTasks.InconsistentTask(), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => RequestProducer.CreateNewRequests(), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate(() => RequestConsumer.ProcessRequests(), Cron.Minutely);

            var stepOne = BackgroundJob.Schedule(() => MockTasks.ChainedTaskOne(), TimeSpan.FromSeconds(20));
            var stepTwo = BackgroundJob.ContinueWith(stepOne, () => MockTasks.ChainedTaskTwo());
            BackgroundJob.ContinueWith(stepTwo, () => MockTasks.ChainedTaskThree());
        }

        public static void EnqueueMockTaskChain()
        {
            var stepOne = BackgroundJob.Schedule(() => MockTasks.ChainedTaskOne(), TimeSpan.FromSeconds(5));
            var stepTwo = BackgroundJob.ContinueWith(stepOne, () => MockTasks.ChainedTaskTwo());
            BackgroundJob.ContinueWith(stepTwo, () => MockTasks.ChainedTaskThree());
        }

        public static void EnqueueMockTask(string text)
        {
            BackgroundJob.Enqueue(() => MockTasks.OneOffTask(text));
        }

        public static void ScheduleDelayedTask(int seconds)
        {
            BackgroundJob.Schedule(() => MockTasks.DelayedTask(), TimeSpan.FromSeconds(30));
        }


        public static void LogStatus(string text, ConsoleColor textColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            Console.ForegroundColor = textColor;
            Console.BackgroundColor = backgroundColor;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        public static void DeleteExistingJobs()
        {

        }
    }
}
