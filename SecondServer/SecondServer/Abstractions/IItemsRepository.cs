using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondServer.Models;

namespace SecondServer.Abstractions
{
    public interface IItemsRepository
    {
        Task<IEnumerable<ToDoItem>> GetItemsAsync(long user);
        Task<IEnumerable<ToDoChange>> GetHistoryAsync(long id,long user);
        Task<bool> AddItem(ToDoItem item);
        Task<ToDoItem> GetItem(long id);
        Task<bool> GetToDoStatus(long id);
        Task<bool> UpdateItem(ToDoItem item, long user);
        Task<bool> DeleteItem(long id, long user);
    }
}
