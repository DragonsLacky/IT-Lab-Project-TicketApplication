using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ticket.Domain;
using Ticket.Repository.Interface;

namespace Ticket.Repository.Implementation
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly ApplicationDbContext _context;
        private DbSet<T> entities;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
            entities = context.Set<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }

        public ValueTask<T> Get(Guid? id)
        {
            return entities.FindAsync(id);
        }

        public void Insert(T entity)
        {
           if(entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Update(entity);
        }

        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }
    }
}
