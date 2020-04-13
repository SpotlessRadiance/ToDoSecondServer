using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondServer.Abstractions
{
    interface IStorage
    {
        T GetRepository<T>() where T : IRepository;
        void Save(); //save changes from all repositories
    }
}
