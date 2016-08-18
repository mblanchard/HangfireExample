using HangfireExample.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireExample.Core
{
    public static class RequestProducer
    {

        public static void CreateNewRequests()
        {
            Entities _context;
            var now = DateTime.Now;
            _context = new Entities(); // TODO: DI
            _context.Requests.AddRange(Enumerable.Range(0, new Random().Next(10)).Select(x => new Request() { Text = String.Format("Task: {0}-{1}", now, x), Key = String.Format("{0}-{1}", now, x), Status=0, IsVisible=true }).ToList());
            _context.SaveChanges();
        }

    }
}
