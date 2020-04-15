using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondServer.Models;

namespace SecondServer.Abstractions
{
    public interface IRepository
    {
        IEnumerable<ToDoItem> Items { get; }
        void AddItem(ToDoItem item);
        ToDoItem GetItem(long id);
        void DeleteItem(ToDoItem item);
        void UpdateItem(ToDoItem item);
    //   void SetStorageContext(IStorageContext storageContext); // save context because all accesses to repository need to be from one context
    }
}
