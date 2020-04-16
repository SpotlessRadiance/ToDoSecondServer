using System;
using System.Collections.Generic;
using System.Linq;
using SecondServer.Abstractions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SecondServer.Models
{
    public class HistoryRepository : IChangesRepository
    {
        private ToDoContext _context;

        public IEnumerable<ToDoChange> Changes => _context.ToDoChanges.ToArray();

        public HistoryRepository(ToDoContext context)
        {
            _context = context;
        }
        public Task<bool> AddLine(ToDoChange change)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ToDoChange>> GetChangesAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<ToDoChange> GetLine(long id)
        {
            return await _context.ToDoChanges
                                 .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
