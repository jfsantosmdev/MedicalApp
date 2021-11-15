using MedicalApp.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedicalApp.DataAccess
{
    public class DbContext<T> : IDbContext<T> where T : class, IEntity
    {
        DbSet<T> _items;

        protected ApiDbContext _ctx;
        public DbContext(ApiDbContext ctx)
        {
            _ctx = ctx;
            _items = ctx.Set<T>();
        }


        public void Delete(int id)
        {
            var tmp = _items.Find(id);
            if (tmp != null)
            {
                _items.Remove(tmp);
                _ctx.SaveChanges();
            }

        }

        public void DeleteAsync(int id)
        {
            var tmp = _items.Find(id);
            if (tmp != null)
            {
                _items.Remove(tmp);
                _ctx.SaveChangesAsync();
            }
        }

        public IQueryable<T> GetAll()
        {
            return _items;
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _items.ToListAsync();
        }

        public T GetById(int id)
        {
            return _items.Find(id);
        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await _items.FindAsync(id);
        }

        public T Save(T entity)
        {
            if (entity.Id.Equals(0))
            {
                _items.Add(entity);
            }
            else
            {
                _items.Update(entity);
            }

            _ctx.SaveChanges();
            return entity;
        }

        public async Task<T> SaveAsync(T entity)
        {
            if (entity.Id.Equals(0))
            {
                await _items.AddAsync(entity);
            }
            else
            {
                _items.Update(entity);
            }

            await _ctx.SaveChangesAsync();
            return entity;
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return GetAll().AsQueryable().Where(conditions);
        }
    }
}
