using Chep.Core;

using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Chep.Data.Repository
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly ChepContext _context;

        public Repository(ChepContext context)
        {
            _context = context;
        }

        public DbSet<T> Set()
        {
            return _context.Set<T>();
        }

        public T Add(T entity)
        {
            Set().Add(entity);

            return entity;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return Set().Any(predicate);
        }

        public void AddRange(List<T> obj)
        {
            Set().AddRange(obj);
        }

        public void Delete(T entity)
        {
            Set().Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var result = Set().Where(predicate);

            foreach (var item in result)
            {
                Set().Remove(item);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<T> GetAll()
        {
            return Set().ToList();
        }

        public List<T> Search(Expression<Func<T, bool>> predicate)
        {
            return Set().Where(predicate).ToList();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return Set().FirstOrDefault(predicate);
        }

        public T Update(T entity)
        {
            Set().Update(entity);

            return entity;
        }
    }
}