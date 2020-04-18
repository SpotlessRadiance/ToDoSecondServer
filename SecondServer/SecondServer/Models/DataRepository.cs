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
        private ToDoContext _context { get; set; }

        public IEnumerable<ToDoItem>Items => _context.ToDoItems.ToArray();

        public DataRepository(ToDoContext context){
            _context = context;
        }

        public async Task<bool> UpdateItem(ToDoItem item, long userId)
        {//обновляются только измененные свойства
            ToDoItem it = await GetItem(item.ID);
            if (userId != it.UserId)
                return false;
            ToDoChange change = new ToDoChange(item);
            it.IsCompleted = item.IsCompleted;
            it.AddChange(change);
            _context.Add(change);
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

        public async Task<IEnumerable<ToDoItem>> GetItemsAsync(long userId)
        {
            return await _context.ToDoItems
                 .Where(c => c.UserId == userId)
                .OrderBy(c => c.RecentUpdate)
                .Include(c=>c.Changes)
                .ToListAsync();
        }

        public async Task<IEnumerable<ToDoChange>> GetHistoryAsync(long id, long userId)
        {
            ToDoItem item = await _context.ToDoItems
                .Where(c=>c.UserId==userId)
                .Include(c=>c.Changes)
                .SingleOrDefaultAsync(c => c.ID == id);

            return item.Changes;
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

        public async Task<bool> DeleteItem(long id, long userId)
        {
            var status = await _context.ToDoItems
                                .Include(c => c.Changes)
                                .SingleOrDefaultAsync(c => c.ID== id);
            if (status.UserId != userId || status == null)
                return false;
            _context.Remove(status);
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
            await _context.SaveChangesAsync();
            ToDoChange change = new ToDoChange(item);
            item.AddChange(change);
            _context.Add(change);
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
