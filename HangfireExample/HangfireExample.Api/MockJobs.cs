using Hangfire;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HangfireExample.Api
{
    public static class MockJobs
    {
        [System.Diagnostics.DebuggerStepThrough]
        [AutomaticRetry(Attempts=10)]
        public static void RunWithTransientErrors()
        {
            if(new Random().NextDouble()>0.5)
            {
                Console.WriteLine("Success");
            }
            else
            {
                throw new Exception("Something went horribly wrong");
            }
        }
    }
}