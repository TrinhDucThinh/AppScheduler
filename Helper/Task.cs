using System;
using System.Collections.Generic;
using System.Text;

namespace AppScheduler.Helper
{
    public class Task
    {
       
        public int Id { get; set; }
        
        public string Action { get; set; }
      
        public DateTime StartTime { get; set; }

        public string Repeat { get; set; }
    }
}
