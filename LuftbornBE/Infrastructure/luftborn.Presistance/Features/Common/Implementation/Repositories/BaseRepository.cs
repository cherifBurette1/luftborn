using Microsoft.EntityFrameworkCore;
using luftborn.Service.Features.Common.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using luftborn.Service.Features.Common.Interfaces;

namespace luftborn.Presistance.Features.Common.Implementation.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        /*
         Please Don't return IQueryable<> result for any public method in any repository,
         Just return List<> to separate services business logic from db logic
         */

        private readonly IluftbornEntities _context;

        public BaseRepository(IluftbornEntities context)
        {
            _context = context;
        }
        protected IluftbornEntities luftbornEntities => _context;
        public async Task Create(T t)
        {
            await luftbornEntities.Set<T>().AddAsync(t);
        }
        public async Task Create(List<T> t)
        {
            await luftbornEntities.Set<T>().AddRangeAsync(t);
        }
        public async Task Remove(T t)
        {
            await Task.Run(() => luftbornEntities.Set<T>().Remove(t));
        }
        public async Task Remove(List<T> t)
        {
            await Task.Run(() => luftbornEntities.Set<T>().RemoveRange(t));
        }
        public async Task<List<T>> FindAll()
        {
            return await luftbornEntities.Set<T>().ToListAsync();
        }
        public async Task<List<T>> Find(Expression<Func<T, bool>> predicate)
        {
            return await luftbornEntities.Set<T>().Where(predicate).ToListAsync();
        }
        public async Task<T> FindOne(Expression<Func<T, bool>> predicate)
        {
            return await luftbornEntities.Set<T>().FirstOrDefaultAsync(predicate);
        }
        public void Attach(T t) => luftbornEntities.Set<T>().Attach(t);
    }
}
