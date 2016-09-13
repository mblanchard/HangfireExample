using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using HangfireExample.Core;

namespace HangfireExample.BackgroundServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverOptions = HangfireHelpers.GenerateServerOptions();
            Console.WriteLine($"Starting server {serverOptions.ServerName}... 'Q' to exit");
            HangfireHelpers.Init(shouldUpdateJobs: false);
            using (var server = new BackgroundJobServer(HangfireHelpers.GenerateServerOptions()))
            {
                var quitKeyPressed = false;
                while (!quitKeyPressed) quitKeyPressed = Console.ReadKey().KeyChar == 'q';               
                HangfireHelpers.LogStatus("Shutting Down Hangfire Server...", ConsoleColor.DarkRed,ConsoleColor.White);
            }
        }
    }
}
