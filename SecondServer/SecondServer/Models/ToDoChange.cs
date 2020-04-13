using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Models
{
    public class ToDoChange
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public DateTime TimeChange { get; set; }

        public ToDoChange() { }
        public ToDoChange(DateTime time, bool status)
        {
            this.Status = status;
            this.TimeChange = time;
        }
    }
}
