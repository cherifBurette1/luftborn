using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace luftborn.Service.Features.Common.Interfaces.Repositories
{
    public interface IBaseRepository<T> where T : class
    {
        Task Create(T t);
        Task Create(List<T> t);
        Task Remove(T t);
        Task Remove(List<T> t);
        Task<List<T>> FindAll();
        Task<List<T>> Find(Expression<Func<T, bool>> predicate);
        Task<T> FindOne(Expression<Func<T, bool>> predicate);
        void Attach(T t);
    }
}
