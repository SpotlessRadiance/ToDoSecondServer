using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Models
{
    public class ToDoChange
    {
        [Key]
        public long Id { get; set; }
        public long ItemId { get; set; }
        public bool Status { get; set; }
        public DateTime TimeChange { get; set; }
    //    public ToDoItem Item { get; set; }

        public ToDoChange() { }
        public ToDoChange( ToDoItem item)
        {
            this.Status = item.IsCompleted;
            this.TimeChange = DateTime.Now;
         //   this.Item = item;
            this.ItemId = item.ID;
        }
    }
}
