using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace ProjectC.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);
        void AddRange(List<T> obj);
        T Update(T entity);
        List<T> GetAll();
        T Single(Expression<Func<T, bool>> predicate);
        List<T> Search(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        void Dispose();
    }
}
