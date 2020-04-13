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
            RecentUpdate = DateTime.Now;
            Changes = new List<ToDoChange>();
        }

        public ToDoItem(long id, bool isCompleted)
        {
            this.ID = id;
            this.IsCompleted = isCompleted;
            RecentUpdate = DateTime.Now;
            Changes = new List<ToDoChange>();
        }

        public void ChangeStatus(bool isCompleted)
        {
            if (this.IsCompleted == isCompleted)
                return;//if status hasn't changed, nothing changes
            this.IsCompleted = isCompleted;
            this.RecentUpdate = DateTime.Now;
            ToDoChange change = new ToDoChange(RecentUpdate, isCompleted);
            Changes.Add(change);
        }
    }
}
