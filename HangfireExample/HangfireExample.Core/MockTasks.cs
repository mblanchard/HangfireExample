using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;

namespace HangfireExample.Core
{
    public static class MockTasks
    {

        public static void OneOffTask(string text)
        {
            HangfireHelpers.LogStatus($"Task {text} Complete", ConsoleColor.White);
        }

        public static void DelayedTask()
        {
            HangfireHelpers.LogStatus("Delayed Task Complete", ConsoleColor.White);
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void SlowTask(IJobCancellationToken cancellationToken)
        {
            var workerId = HangfireHelpers.ClaimWorker("Slow Task");
            if (workerId == -1) return;
            HangfireHelpers.LogStatus("Slow Task Started", ConsoleColor.White);
            try
            {
                for (int i = 0; i < 20; i++)
                {
                    Thread.Sleep(1000);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                HangfireHelpers.LogStatus("Slow Task Complete", ConsoleColor.White);
                HangfireHelpers.FreeWorker(workerId);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void SlowerTask(IJobCancellationToken cancellationToken)
        {
            var workerId = HangfireHelpers.ClaimWorker("Slower Task");
            if (workerId == -1) return;
            HangfireHelpers.LogStatus("Slower Task Started", ConsoleColor.White);
            try
            {
                for (int i = 0; i < 180; i++)
                {
                    Thread.Sleep(1000);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                HangfireHelpers.LogStatus("Slower Task Complete", ConsoleColor.White);
                HangfireHelpers.FreeWorker(workerId);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        public static void SlowestTask(IJobCancellationToken cancellationToken)
        {
            var workerId = HangfireHelpers.ClaimWorker("Slowest Task");
            if (workerId == -1) return;
            HangfireHelpers.LogStatus("Slowest Task Started", ConsoleColor.White);
            try
            {
                for (int i = 0; i < 1800; i++)
                {
                    Thread.Sleep(1000);
                    cancellationToken.ThrowIfCancellationRequested();
                }
            }
            finally
            {
                HangfireHelpers.LogStatus("Slowest Task Complete", ConsoleColor.White);
                HangfireHelpers.FreeWorker(workerId);
            }
        }

        [System.Diagnostics.DebuggerStepThrough]
        [AutomaticRetry(Attempts = 10)]
        public static void InconsistentTask()
        {
            if (new Random().NextDouble() > 0.5)
            {
                HangfireHelpers.LogStatus("Inconsistent Task Succeeded", ConsoleColor.Green);
            }
            else
            {
                HangfireHelpers.LogStatus("Inconsistent Task Failed. Retrying...", ConsoleColor.Red);
                throw new Exception("Something went horribly wrong");
            }
        }


        public static void ChainedTaskOne()
        {
            HangfireHelpers.LogStatus("Chained Tasks: Step One", ConsoleColor.Gray);
        }

        public static void ChainedTaskTwo()
        {
            HangfireHelpers.LogStatus("Chained Tasks: Step Two", ConsoleColor.Blue, ConsoleColor.Gray);
        }

        public static void ChainedTaskThree()
        {
            HangfireHelpers.LogStatus("Chained Tasks: Step Three", ConsoleColor.Blue, ConsoleColor.White);
        }
    }
}
