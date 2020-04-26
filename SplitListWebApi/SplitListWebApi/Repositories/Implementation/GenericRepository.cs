using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiFormat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Interfaces;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Repositories.Implementation
{
    public class GenericRepository<TModel> : IRepository<TModel>
        where TModel : class, IModel
    {
        private readonly SplitListContext _dbContext;
        public GenericRepository(SplitListContext dbContext)
        {
            _dbContext = dbContext;
        }

        public TModel GetBy(
            Expression<Func<TModel, TModel>> selector,
            Expression<Func<TModel, bool>> predicate = null,
            Func<IQueryable<TModel>, IOrderedQueryable<TModel>> orderBy = null,
            Func<IQueryable<TModel>, IIncludableQueryable<TModel, object>> include = null,
            bool disableTracking = true)
        {
            return GeneralUtilities.GetFromDatabase(_dbContext, selector, predicate, orderBy, include, disableTracking);
        }

        public TModel Create(TModel model)
        {
            return model.WriteToDatabase(_dbContext.Add, _dbContext);
        }

        public TModel Update(TModel model)
        {
            return model.WriteToDatabase(_dbContext.Update, _dbContext);
        }

        public void Delete(TModel model)
        {
            model.WriteToDatabase(_dbContext.Remove, _dbContext);
        }
    }
}