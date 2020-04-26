using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiFormat;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
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

        public TModel GetBy(Expression<Func<TModel, bool>> predicate) => GeneralUtilities.GetFromDatabase(_dbContext, predicate);

        public TModel Create(TModel model)
        {
            //_dbContext.Entry(model).State = EntityState.Detached;
            return model.WriteToDatabase(_dbContext.Add, _dbContext);
        }

        public TModel Update(TModel model)
        {
            _dbContext.Entry(model).State = EntityState.Detached;
            return model.WriteToDatabase(_dbContext.Update, _dbContext); ; //To check whether any entries has been updated. Look in DTOUtilities.Update.
        }

        public void Delete(TModel model)
        {
            model.WriteToDatabase(_dbContext.Remove, _dbContext);
        }
    }
}