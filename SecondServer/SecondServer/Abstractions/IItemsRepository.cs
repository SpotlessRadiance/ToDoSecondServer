using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondServer.Models;

namespace SecondServer.Abstractions
{
    public interface IItemsRepository
    {
        Task<IEnumerable<ToDoItem>> GetItemsAsync();
        Task<IEnumerable<ToDoChange>> GetHistoryAsync(long id);
        Task<bool> AddItem(ToDoItem item);
        Task<ToDoItem> GetItem(long id);
        Task<bool> GetToDoStatus(long id);
        Task<bool> UpdateItem(ToDoItem item);
        Task<bool> DeleteItem(long id);
    }
}
