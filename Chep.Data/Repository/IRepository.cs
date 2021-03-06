using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Chep.Data.Repository
{
    public interface IRepository<T> where T : class
    {
        DbSet<T> Set();
        T Add(T entity);
        List<T> AddRange(List<T> obj);
        T Update(T entity);
        List<T> GetAll();
        T Single(Expression<Func<T, bool>> predicate);
        List<T> Search(Expression<Func<T, bool>> predicate);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> predicate);
        bool Any(Expression<Func<T, bool>> predicate);
        void Dispose();
        List<T> UpdateRange(List<T> obj);
    }
}