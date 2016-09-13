using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HangfireExample.Core;

namespace HangfireExample.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            HangfireHelpers.Init(shouldUpdateJobs: true);

            Console.WriteLine(@"    __  __                  _____              ____ ");
            Console.WriteLine(@"   / / / /___ _____  ____ _/ __(_)_______     / __ \___  ____ ___  ____ ");
            Console.WriteLine(@"  / /_/ / __ `/ __ \/ __ `/ /_/ / ___/ _ \   / / / / _ \/ __ `__ \/ __ \");
            Console.WriteLine(@" / __  / /_/ / / / / /_/ / __/ / /  /  __/  / /_/ /  __/ / / / / / /_/ /");
            Console.WriteLine(@"/_/ /_/\__,_/_/ /_/\__, /_/ /_/_/   \___/  /_____/\___/_/ /_/ /_/\____/ ");
            Console.WriteLine(@"                  /____/");
            HangfireHelpers.LogStatus("\nTry {E}nqeue, {D}elayed, {C}hain, or {Q}uit", ConsoleColor.Yellow);

            char key;
            do
            {
                key = Console.ReadKey().KeyChar;
                switch (key)
                {
                    case 'e': HangfireHelpers.LogStatus("\t{E}nqueud new task", ConsoleColor.White); HangfireHelpers.EnqueueMockTask("One-off"); break;
                    case 'd': HangfireHelpers.LogStatus("\tEnqueued {D}elayed task", ConsoleColor.White); HangfireHelpers.ScheduleDelayedTask(30); break;
                    case 'c': HangfireHelpers.LogStatus("\tEnqueued task {C}hain", ConsoleColor.White); HangfireHelpers.EnqueueMockTaskChain(); break;
                    case 'q': HangfireHelpers.LogStatus("\tShutting Down Hangfire Server...", ConsoleColor.DarkRed, ConsoleColor.White); break;
                    default:  HangfireHelpers.LogStatus("\tTry {E}nqeue, {D}elayed, {C}hain, or {Q}uit", ConsoleColor.Yellow); break;
                }
            }
            while (key != 'q');
        }
    }
}
