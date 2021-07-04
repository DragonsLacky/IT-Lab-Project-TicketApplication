using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain;

namespace Ticket.Repository.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        ValueTask<T> Get(Guid? id);
        
        void Insert(T entity);
        
        void Update(T entity);
        
        void Delete(T entity);
                
        void SaveChanges();
    }
}
