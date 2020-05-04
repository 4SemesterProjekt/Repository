using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiFormat;

namespace SplitListWebApi.Services.Interfaces
{
    public interface IService<TSource, T>
        where TSource : class, IDTO
    {
        List<TSource> GetAll();
        TSource GetBy(Expression<Func<T, bool>> predicate);
        TSource Create(TSource dto);
        TSource Update(TSource dto);
        void Delete(TSource dto);
    }
}