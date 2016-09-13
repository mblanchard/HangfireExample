using System;
using Hangfire;
using Hangfire.SqlServer;
using HangfireExample.Core;

namespace HangfireExample.BackgroundServer
{
    class Program
    {
        static void Main(string[] args)
        {
            
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireExample", new SqlServerStorageOptions { QueuePollInterval = TimeSpan.FromSeconds(1) });
            StartBackgroundJobServer();
           
        }

        static void StartBackgroundJobServer()
        {
            var serverOptions = HangfireHelpers.GenerateServerOptions();
            SetupStatus(serverOptions);
            using (var server = new BackgroundJobServer(serverOptions))
            {
                var quitKeyPressed = false;
                while (!quitKeyPressed) quitKeyPressed = Console.ReadKey().KeyChar == 'q';
                HangfireHelpers.LogStatus("Shutting Down Hangfire Server...", ConsoleColor.DarkRed, ConsoleColor.White);
            }
        }

        static void SetupStatus(BackgroundJobServerOptions serverOptions)
        {
            Console.WriteLine($"Starting server {serverOptions.ServerName}... 'Q' to exit\n\nWorker\tStatus\n-------------------");
            HangfireHelpers.InitWorkerStatus();
        }
    }
}
