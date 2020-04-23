using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiFormat;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace SplitListWebApi.Repositories.Interfaces
{
    public interface IRepository<TModel>
    {
        TModel GetBy(Expression<Func<TModel, bool>> predicate);
        TModel Create(TModel entity);
        TModel Update(TModel model);
        void Delete(TModel entity);
    }
}