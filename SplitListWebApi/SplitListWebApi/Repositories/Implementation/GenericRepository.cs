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
    public class GenericRepository<TDTO, TModel> : IRepository<TDTO>
        where TDTO : class, IDTO
        where TModel : class, IModel
    {
        private readonly SplitListContext _dbContext;
        private IMapper _mapper;
        public GenericRepository(SplitListContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public IQueryable<TDTO> GetAll() => _mapper.Map<IQueryable<TDTO>>(_dbContext.Set<TModel>().AsNoTracking());

        public TDTO GetById(int id) => _mapper.Map<TModel, TDTO>(id.GetFromDatabase<TModel>(_dbContext));

        public TDTO Create(TDTO entity)
        {
            var model = _mapper.Map<TModel>(entity);
            model = model.WriteToDatabase(_dbContext.Add, _dbContext);
            return _mapper.Map<TDTO>(model);
        }

        public TDTO Update(TDTO dto)
        {
            /*
            * Map from TSource to TEntity
            * Get TEntity from DB
            * Assign mapped TEntity to DB_TEntity
            * Update DB_TEntity
            * Map from TEntity to TSource
            */

            var model = dto.ModelId.GetFromDatabase<TModel>(_dbContext);
            model = _mapper.Map<TModel>(dto);
            model.WriteToDatabase(_dbContext.Update, _dbContext);
            return _mapper.Map<TDTO>(model); //To check whether any entries has been updated. Look in DTOUtilities.Update.
        }

        public void Delete(TDTO entity)
        {
            var model = _mapper.Map<TModel>(entity);
            model.WriteToDatabase(_dbContext.Remove, _dbContext);
        }
    }
}