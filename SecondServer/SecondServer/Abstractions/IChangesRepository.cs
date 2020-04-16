using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SecondServer.Models;

namespace SecondServer.Abstractions
{
    public interface IChangesRepository
    {
        Task<IEnumerable<ToDoChange>> GetChangesAsync();
       Task<bool> AddLine(ToDoChange change);
        Task<ToDoChange> GetLine(long id);
    }
}
