using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Models
{
    public class ToDoItem
    {
        
        public long ID { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime RecentUpdate { get; protected set; }
        public List<ToDoChange> Changes { get; protected set; }

        public ToDoItem() {
            IsCompleted = false;
            RecentUpdate = DateTime.Now;
            Changes = new List<ToDoChange>();
        }

        public ToDoItem(bool isCompleted)
        {
            this.IsCompleted = isCompleted;
            RecentUpdate = DateTime.Now;
            Changes = new List<ToDoChange>();
        }

        public void AddChange(ToDoChange change)
        {
            this.IsCompleted = change.Status;
            this.RecentUpdate = DateTime.Now;
            Changes.Add(change);
        }
    }
}
