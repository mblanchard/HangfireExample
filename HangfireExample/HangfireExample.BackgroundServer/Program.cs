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
            Console.WindowHeight = 20;
            // Hangfire storage configuration
            GlobalConfiguration.Configuration.UseSqlServerStorage("HangfireExample", new SqlServerStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(1) //Default is 15 seconds
            });

            StartBackgroundJobServer();
           
        }

        static void StartBackgroundJobServer()
        {
            var serverOptions = HangfireHelpers.GenerateServerOptions();
            SetupStatus(serverOptions);
            using (var server = new BackgroundJobServer(serverOptions)) //Start Hangfire BackgroundJobServer, with demo-specific config
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
