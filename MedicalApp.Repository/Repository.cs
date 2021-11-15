using MedicalApp.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedicalApp.Repository
{
    public interface IRepository<T> : ICrud<T> { }
    public class Repository<T> : IRepository<T> where T : IEntity
    {
        IDbContext<T> _ctx;
        public Repository(IDbContext<T> ctx)
        {
            _ctx = ctx;
        }
        public void Delete(int id)
        {
            _ctx.Delete(id);
        }

        public void DeleteAsync(int id)
        {
            _ctx.DeleteAsync(id);
        }

        public IQueryable<T> GetAll()
        {
            return _ctx.GetAll();
        }

        public async Task<IList<T>> GetAllAsync()
        {
            return await _ctx.GetAllAsync();
        }

        public T GetById(int id)
        {
            return _ctx.GetById(id);
        }

        public Task<T> GetByIdAsync(int id)
        {
            return _ctx.GetByIdAsync(id);
        }

        public T Save(T entity)
        {
            return _ctx.Save(entity);
        }

        public Task<T> SaveAsync(T entity)
        {
            return _ctx.SaveAsync(entity);
        }

        public IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions)
        {
            return _ctx.GetAll().AsQueryable().Where(conditions);
        }
    }
}
