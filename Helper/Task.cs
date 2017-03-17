using System;
using System.Collections.Generic;
using System.Text;

namespace AppScheduler.Helper
{
    public class Task
    {
       
        public int TaskId { get; set; }

        public string Name { get; set; }

        public string Action { get; set; }

        public DateTime StartDate { get; set; }

        public string RunTime { get; set; }

        public DateTime NextTime { get; set; }

        public string Type { get; set; }
      
        public string Recur { get; set; }

        public string Detail { get; set; }

        public string  Status { get; set; }
    }
}
