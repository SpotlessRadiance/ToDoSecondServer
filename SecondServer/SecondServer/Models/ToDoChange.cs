using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Models
{
    public class ToDoChange
    {
        public long Id { get; set; }
        [Required]
        public long ItemId { get; set; }
        public bool Status { get; set; }
        public DateTime TimeChange { get; set; }

        public ToDoChange() { }
        public ToDoChange( bool status, long item_id)
        {
            this.Status = status;
            this.TimeChange = DateTime.Now;
            this.ItemId = item_id;
        }
    }
}
