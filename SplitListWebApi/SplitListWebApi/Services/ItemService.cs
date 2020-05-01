using ApiFormat;
using AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat.Item;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class ItemService : IService<ItemDTO, int>

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

        public ItemDTO GetById(int id)
        {
            return _mapper.Map<ItemDTO>(itemRepo.GetBy(
                selector: source => source,
                predicate: model => model.ModelId == id,
                disableTracking: false));
        }

        public ItemDTO Create(ItemDTO dto)
        {
            var model = _mapper.Map<ItemModel>(dto);
            var dbModel = itemRepo.GetBy(
                selector: source => source,
                predicate: slm => slm.Name == model.Name,
                disableTracking: false);

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
            var dbModel = itemRepo.GetBy(
                selector: source => source,
                predicate: slm => slm.ModelId == model.ModelId,
                disableTracking: false);

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
            var dbModel = itemRepo.GetBy(
                selector: source => source,
                predicate: gm => gm.ModelId == model.ModelId,
                disableTracking: false);

            if (dbModel == null) throw new NullReferenceException("ItemDTO wasn't found in the database when trying to delete.");

            itemRepo.Delete(dbModel);
        }




    }
}
