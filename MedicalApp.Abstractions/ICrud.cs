using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace MedicalApp.Abstractions
{
    public interface ICrudAsync<T>
    {
        Task<T> SaveAsync(T entity);
        Task<IList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void DeleteAsync(int id);
    }

    public interface ICrud<T> : ICrudAsync<T>
    {
        T Save(T entity);
        IQueryable<T> GetAll();
        T GetById(int id);
        void Delete(int id);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> conditions);
    }

}
