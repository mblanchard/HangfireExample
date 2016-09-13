using HangfireExample.DAL;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HangfireExample.Core
{
    public static class RequestConsumer
    {
        public static void ProcessRequests()
        {
            using (HangfireExampleContext _context = new HangfireExampleContext())
            {
                var nextReq = _context.Requests.FirstOrDefault(x => x.Status == 0);
                if (nextReq == null) return;
                nextReq.Status = 1;
                _context.Entry(nextReq).State = System.Data.Entity.EntityState.Modified;
                _context.SaveChanges();
                HangfireHelpers.LogStatus($"Processed Request {nextReq.Name}, Created at {nextReq.TimeCreated.ToLocalTime()}", ConsoleColor.Yellow);
            }
        }
    }
}
