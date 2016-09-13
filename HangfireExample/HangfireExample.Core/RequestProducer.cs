using HangfireExample.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HangfireExample.Core
{
    public class RequestProducer
    {
        public void CreateNewRequests()
        {
            using (HangfireExampleContext _context = new HangfireExampleContext())
            {
                var now = DateTime.Now;
                _context.Requests.AddRange(Enumerable.Range(0, new Random().Next(10)).Select(x => new Request() { Name =  $"Task: {now}-{x}",  Status = 0, TimeCreated = DateTimeOffset.Now}).ToList());
                _context.SaveChanges();
                HangfireHelpers.LogStatus("New Requests Created", ConsoleColor.Yellow);
            }
        }

    }
}
