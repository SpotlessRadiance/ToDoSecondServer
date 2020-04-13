using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecondServer.Abstractions;

namespace SecondServer.Models
{
    public class DataRepository : IItemsRepository
    {//сложные запросы будут объединятся в этом классе, тк детали работы с БД скрываются от остального приложения
        private ToDoContext _context;

        public IEnumerable<ToDoItem>Items => _context.ToDoItems.ToArray();

        public DataRepository(ToDoContext context){
            _context = context;
        }

        public async Task<bool> UpdateItem(ToDoItem item)
        {//обновляются только измененные свойства
            ToDoItem it = await GetItem(item.ID);
            it.IsCompleted = item.IsCompleted;
            it.ChangeStatus(item.IsCompleted);
            _context.Entry(it).State = EntityState.Modified;
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (Exception exp)
            {
                return false;
            }
        }

        public async Task<IEnumerable<ToDoItem>> GetItemsAsync()
        {
            return await _context.ToDoItems.OrderBy(c => c.RecentUpdate).ToListAsync();
        }

        public async Task<ToDoItem> GetItem(long id)
        {
            return await _context.ToDoItems
                                 .SingleOrDefaultAsync(c => c.ID == id);
        }

        public async Task<bool> GetToDoStatus(long id)
        {
            ToDoItem item = await _context.ToDoItems.SingleOrDefaultAsync(c => c.ID == id);
            return item.IsCompleted;
        }

        public async Task<bool> DeleteItem(long id)
        {
            var customer = await _context.ToDoItems
                                .Include(c => c.Changes)
                                .SingleOrDefaultAsync(c => c.ID== id);
            _context.Remove(customer);
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            { return false; }
        }

        public async Task<bool> AddItem(ToDoItem item)
        {
            _context.Add(item);
            try
            {
                return (await _context.SaveChangesAsync() > 0 ? true : false);
            }
            catch (System.Exception exp)
            {
                return false;
            }
        }
    }
}
