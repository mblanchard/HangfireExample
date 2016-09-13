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
        private static object _writeLock = new object(); //naive spinlock for thread-safe logging
        private const int DEFAULT_WORKER_COUNT = 4;
        private static string[] _workerStatus = new string[DEFAULT_WORKER_COUNT];
        public static  BackgroundJobServerOptions GenerateServerOptions()
        {
            return new BackgroundJobServerOptions
            {
                ServerName = String.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString()),
                WorkerCount = DEFAULT_WORKER_COUNT //Low WorkerCount to force distributed task load
            }; 
        }
        
        /// <summary>
        /// Creates/updates mock recurring tasks
        /// </summary>
        public static void AddOrUpdateRecurringMockTasks()
        {
            //Invoking static methods
            RecurringJob.AddOrUpdate(() => MockTasks.SlowTask(JobCancellationToken.Null), Cron.Minutely);
            RecurringJob.AddOrUpdate(() => MockTasks.SlowerTask(JobCancellationToken.Null), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate(() => MockTasks.SlowestTask(JobCancellationToken.Null), Cron.Hourly);
            RecurringJob.AddOrUpdate(() => MockTasks.InconsistentTask(), Cron.Minutely);

            //Invoking instance methods
            RecurringJob.AddOrUpdate<RequestProducer>(x => x.CreateNewRequests(), Cron.MinuteInterval(5));
            RecurringJob.AddOrUpdate<RequestConsumer>(x => x.ProcessRequests(), Cron.Minutely);
        }

        /// <summary>
        /// Schedules a task chain, to begin in 5 seconds
        /// </summary>
        public static void EnqueueMockTaskChain()
        {
            var stepOne = BackgroundJob.Schedule(() => MockTasks.ChainedTaskOne(), TimeSpan.FromSeconds(5));
            var stepTwo = BackgroundJob.ContinueWith(stepOne, () => MockTasks.ChainedTaskTwo());
            BackgroundJob.ContinueWith(stepTwo, () => MockTasks.ChainedTaskThree());
        }

        /// <summary>
        /// Queues up a one-off task
        /// </summary>
        /// <param name="text"></param>
        public static void EnqueueMockTask(string text)
        {
            BackgroundJob.Enqueue(() => MockTasks.OneOffTask(text));
        }

        /// <summary>
        /// Schedules a task X seconds in the future
        /// </summary>
        /// <param name="seconds">Delay before task runs</param>
        public static void ScheduleDelayedTask(int seconds)
        {
            BackgroundJob.Schedule(() => MockTasks.DelayedTask(), TimeSpan.FromSeconds(30));
        }

        public static int ClaimWorker(string text)
        {
            if (string.IsNullOrEmpty(text)) return -1;
            lock (_writeLock)
            {
                for (var i = 0; i < DEFAULT_WORKER_COUNT; i++)
                {
                    if (string.IsNullOrEmpty(_workerStatus[i])) { writeWorkerStatus(text, i); return i; }
                }
                return -1;
            }
        }

        public static void FreeWorker(int workerId)
        {
            lock (_writeLock)
            {
                writeWorkerStatus(string.Empty,workerId);
            }

        }

        public static void InitWorkerStatus()
        {
            lock (_writeLock)
            {
                for (int i = 0; i < DEFAULT_WORKER_COUNT; i++)
                {
                    Console.SetCursorPosition(0, i + 4);
                    Console.Write(i+1);
                    writeWorkerStatus("", i);
                }
            }
        }

        private static void writeWorkerStatus(string text, int workerId)
        {
            _workerStatus[workerId] = text;
            if(String.IsNullOrEmpty(text))
            {
                text = "Open"; Console.ForegroundColor = ConsoleColor.Green;
            }
            Console.SetCursorPosition(2, workerId + 4);
            Console.Write($"\t{text.PadRight(20)}");
            Console.ResetColor();
        }



        public static void LogStatus(string text, ConsoleColor textColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            lock (_writeLock)
            {
                Console.BackgroundColor = backgroundColor;
                Console.SetCursorPosition(0,DEFAULT_WORKER_COUNT + 7);
                Console.WriteLine($"Last Update ({DateTime.Now})".PadRight(90));
                Console.ForegroundColor = textColor;
                Console.WriteLine(text.PadRight(90));
                Console.ResetColor();
            }
        }
    }
}
