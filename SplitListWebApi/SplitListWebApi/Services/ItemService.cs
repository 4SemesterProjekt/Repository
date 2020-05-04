using ApiFormat;
using AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ApiFormat.Item;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class ItemService : IPublicService<ItemDTO, ItemModel>, IModelService<ItemDTO, ItemModel>

    {
        private SplitListContext _context;
        private IMapper _mapper;
        private GenericRepository<ItemModel> itemRepo;

        public ItemService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            itemRepo = new GenericRepository<ItemModel>(_context);
        }

        public IEnumerable<ItemModel> GetModels(Expression<Func<ItemModel, bool>> predicate, bool disableTracking = true)
        {
            return itemRepo.GetBy(
                selector: source => source,
                predicate: predicate,
                disableTracking: disableTracking);
        }

        public List<ItemDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public ItemDTO GetBy(Expression<Func<ItemModel, bool>> predicate)
        {
            return _mapper.Map<ItemDTO>(GetModels(predicate).FirstOrDefault());
        }

        public ItemDTO Create(ItemDTO dto)
        {
            var model = _mapper.Map<ItemModel>(dto);
            var dbModel = GetModels(source => source.Name == model.Name, false).FirstOrDefault();

            if (dbModel == null)
            {
                ItemDTO nullItemResult = _mapper.Map<ItemDTO>(itemRepo.Create(model));
                nullItemResult.Amount = dto.Amount;
                return nullItemResult;
            }
            ItemDTO result = _mapper.Map<ItemDTO>(dbModel);
            result.Amount = dto.Amount;
            return result;
        }

        public ItemDTO Update(ItemDTO dto)
        {
            var model = _mapper.Map<ItemModel>(dto);
            var dbModel = GetModels(source => source.Name == model.Name, false).FirstOrDefault();

            if (dbModel == null)
            {
                ItemDTO nullItemResult = _mapper.Map<ItemDTO>(itemRepo.Create(model));
                nullItemResult.Amount = dto.Amount;
                return nullItemResult; 
            }

            dbModel.Name = model.Name;
            dbModel.Type = model.Type;
            dbModel = itemRepo.Update(dbModel);
            ItemDTO result = _mapper.Map<ItemDTO>(dbModel);
            result.Amount = dto.Amount;
            return result;
        }

        public void Delete(ItemDTO dto)
        {
            var model = _mapper.Map<ItemModel>(dto);
            var dbModel = GetModels(source => source.Name == model.Name, false).FirstOrDefault();

            if (dbModel == null) throw new NullReferenceException("ItemDTO wasn't found in the database when trying to delete.");

            itemRepo.Delete(dbModel);
        }
    }
}
