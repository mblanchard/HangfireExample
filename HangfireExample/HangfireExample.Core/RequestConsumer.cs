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
            Entities _context;
            var now = DateTime.Now;
            _context = new Entities(); // TODO: DI
            var nextReq = _context.Requests.Where(x => x.Status == 0).FirstOrDefault();
            if (nextReq == null) return;
            nextReq.Status = 1;
            _context.Entry(nextReq).State = System.Data.Entity.EntityState.Modified;
            _context.SaveChanges();


        }
    }
}
