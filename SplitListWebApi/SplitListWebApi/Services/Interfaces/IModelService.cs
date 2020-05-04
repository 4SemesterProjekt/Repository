using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using ApiFormat;

namespace SplitListWebApi.Services.Interfaces
{
    public interface IModelService<TSource, T>
    {
        IEnumerable<T> GetModels(Expression<Func<T, bool>> predicate, bool disableTracking = true);
        List<TSource> GetAll();
        TSource GetBy(Expression<Func<T, bool>> predicate);
        TSource Create(TSource dto);
        TSource Update(TSource dto);
        void Delete(TSource dto);
    }
}