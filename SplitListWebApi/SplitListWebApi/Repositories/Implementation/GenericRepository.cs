using System;
using System.Linq;
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
    public class GenericRepository<TSource, TEntity> : IRepository<TSource>
        where TSource : class, IDTO
        where TEntity : class, IModel
    {
        private readonly SplitListContext _dbContext;
        private IMapper _mapper;
        public GenericRepository(SplitListContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<TSource> GetAll() => _mapper.Map<IQueryable<TSource>>(_dbContext.Set<TEntity>().AsNoTracking());

        public TSource GetById(double id) => id.GetFromDatabase<TSource, TEntity>(_dbContext, _mapper);

        public TSource Create(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.WriteToDatabase(_dbContext.Add, _dbContext);
            return _mapper.Map<TSource>(model);
        }

        public TSource Update(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.WriteToDatabase(_dbContext.Update, _dbContext);
            return _mapper.Map<TSource>(model); //To check whether any entries has been updated. Look in DTOUtilities.Update.
        }

        public void Delete(TSource entity)
        {
            var model = _mapper.Map<TEntity>(entity);
            model.WriteToDatabase(_dbContext.Remove, _dbContext);
        }
    }
}