using ProjectC.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace ProjectC.Data.Repository
{
    public class Repository<T> : IRepository<T>, IDisposable where T : class
    {
        private readonly Chep_NewContext _context;

        public Repository(Chep_NewContext context)
        {
            _context = context;
        }

        public T Add(T entity)
        {
            _context.Set<T>().Add(entity);
            return entity;
        }

        public bool Any(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Any(predicate);
        }

        public void AddRange(List<T> obj)
        {
            _context.Set<T>().AddRange(obj);
        }

        public void Delete(T entity)
        {
            _context.Set<T>().Remove(entity);
        }

        public void Delete(Expression<Func<T, bool>> predicate)
        {
            var result = _context.Set<T>().Where(predicate);

            foreach (var item in result)
            {
                _context.Set<T>().Remove(item);
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public List<T> GetAll()
        {
            return _context.Set<T>().ToList();
        }

        public List<T> Search(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().Where(predicate).ToList();
        }

        public T Single(Expression<Func<T, bool>> predicate)
        {
            return _context.Set<T>().FirstOrDefault(predicate);
        }

        public T Update(T entity)
        {
            _context.Set<T>().Update(entity);
            return entity;
        }
    }
}
