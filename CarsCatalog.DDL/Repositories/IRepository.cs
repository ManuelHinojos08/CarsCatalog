using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarsCatalog.DDL.Repositories
{
    //The Generic Interface Repository for Performing Read/Add/Delete operations
    internal interface IRepository<TEnt, in TPk> where TEnt : class
    {
        IEnumerable<TEnt> Get();
        TEnt Get(TPk id);
        string Add(TEnt entity, out bool saved);        


    }
}
