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

        public static void SlowTask()
        {
            HangfireHelpers.LogStatus("Slow Task Started", ConsoleColor.White);
            Thread.Sleep(20000); //I'm sorry
            HangfireHelpers.LogStatus("Slow Task Complete", ConsoleColor.White);
        }

        public static void SlowerTask()
        {
            HangfireHelpers.LogStatus("Slower Task Started", ConsoleColor.White);
            Thread.Sleep(180000); //I'm 9x sorrier
            HangfireHelpers.LogStatus("Slower Task Complete", ConsoleColor.White);
        }

        public static void SlowestTask()
        {
            HangfireHelpers.LogStatus("Slowest Task Started", ConsoleColor.White);
            Thread.Sleep(1800000); //I'm sorriest
            HangfireHelpers.LogStatus("Slowest Task Complete", ConsoleColor.White);
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
