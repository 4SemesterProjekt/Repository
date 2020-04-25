using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.ShoppingList;
using AutoMapper;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class ShoppingListService : IService<ShoppingListDTO, int>
    {
        private SplitListContext _context;
        private readonly IMapper _mapper;
        private readonly GenericRepository<ShoppingListModel> _shoppingListRepository;
        private readonly ShoppingListItemRepository _shoppingListItemRepo;

        public ShoppingListService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _shoppingListRepository = new GenericRepository<ShoppingListModel>(context);
            _shoppingListItemRepo = new ShoppingListItemRepository(context);
        }


        public ShoppingListDTO GetById(int id)
        {
            return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.GetBy(model => model.ModelId == id));
        }

        public ShoppingListDTO Create(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = _shoppingListRepository.GetBy(m => m.ModelId == dto.ModelId);

            if (dbModel == null)
            {
                var createdShoppingListModel = _shoppingListRepository.Create(model);
                var itemModels = _mapper.Map<List<ItemModel>>(dto.Items);
                _shoppingListItemRepo.CreateShoppingListItems(itemModels, createdShoppingListModel);

                dbModel = _shoppingListRepository.GetBy(model => model.ModelId == createdShoppingListModel.ModelId);
                dbModel.ShoppingListItems = _shoppingListItemRepo.GetBy(dbModel.ModelId, itemModels.Select(im => im.ModelId));
                return _mapper.Map<ShoppingListDTO>(dbModel);
            }

            return _mapper.Map<ShoppingListDTO>(dbModel);
        }

        public ShoppingListDTO Update(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = _shoppingListRepository.GetBy(model => model.ModelId == dto.ModelId);
            if (dbModel == null)
                return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.Create(model));

            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListItemRepo.CreateShoppingListItems(_mapper.Map<List<ItemModel>>(dto.Items), dbModel);
            return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.GetBy(m => m.ModelId == dbModel.ModelId));
        }

        public void Delete(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListDTO>(dto);
            var dbModel = _shoppingListRepository.GetBy(model => model.ModelId == dto.ModelId);
            if (dbModel == null) return;

            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListRepository.Delete(dbModel);
        }
    }
}