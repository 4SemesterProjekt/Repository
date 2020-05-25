using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Repositories.Interfaces;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class ShoppingListService : IPublicService<ShoppingListDTO, ShoppingListModel>, IModelService<ShoppingListDTO, ShoppingListModel>
    {
        private readonly IMapper _mapper;
        private readonly IRepository<ShoppingListModel> _shoppingListRepository;
        private readonly IRepository<GroupModel> _groupRepository;
        private readonly ShoppingListItemRepository _shoppingListItemRepo;
        private readonly IPublicService<ItemDTO, ItemModel> _itemService;
        public ShoppingListService(SplitListContext context, IMapper mapper)
        {
            _mapper = mapper;
            _shoppingListRepository = new GenericRepository<ShoppingListModel>(context);
            _groupRepository = new GenericRepository<GroupModel>(context);
            _shoppingListItemRepo = new ShoppingListItemRepository(context);
            _itemService = new ItemService(context, mapper);
        }


        public List<ShoppingListDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public ShoppingListDTO GetBy(Expression<Func<ShoppingListModel, bool>> predicate)
        {
            return _mapper.Map<ShoppingListDTO>(GetModels(predicate).FirstOrDefault());
        }

        public IEnumerable<ShoppingListModel> GetModels(Expression<Func<ShoppingListModel, bool>> predicate,
            bool disableTracking = true)
        {
            return _shoppingListRepository.GetBy(
                selector: source => source,
                predicate: predicate,
                include: source =>
                    source.Include(slm => slm.ShoppingListItems)
                        .ThenInclude(sli => sli.ItemModel)
                        .Include(slm => slm.GroupModel),
                disableTracking: disableTracking);
        }

        public ShoppingListDTO Create(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = GetModels(source => source.ModelId == model.ModelId, false).FirstOrDefault();

            if (dbModel != null) return _mapper.Map<ShoppingListDTO>(dbModel);

            if (dto.Group.ModelId != 0)
            {
                model.GroupModel = _groupRepository.GetBy(
                    selector: source => source,
                    predicate: gm => gm.ModelId == dto.Group.ModelId,
                    disableTracking: false)
                    .FirstOrDefault();
            }

            dbModel = _shoppingListRepository.Create(model);

            if (dto.Items != null)
            {
                for (int i = 0; i < dto.Items.Count; i++) //Sets Ids on dtos
                {
                    dto.Items[i] = _itemService.Create(dto.Items[i]);
                }

                _shoppingListItemRepo.CreateShoppingListItems(dto.Items, dbModel);
            }

            var tempModel = dbModel;

            dbModel = GetModels(source => source.ModelId == model.ModelId).FirstOrDefault();

            return _mapper.Map<ShoppingListDTO>(dbModel);
        }

        public ShoppingListDTO Update(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = GetModels(source => source.ModelId == model.ModelId, false).FirstOrDefault();

            if (dbModel == null)
                return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.Create(model));

            dbModel.Name = dto.Name;
            _shoppingListRepository.Update(dbModel);

            if (dto.Items != null)
            {
                for (int i = 0; i < dto.Items.Count; i++)
                {
                    dto.Items[i] = _itemService.Create(dto.Items[i]);
                }
            }
            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListItemRepo.CreateShoppingListItems(dto.Items, dbModel);
            return _mapper.Map<ShoppingListDTO>(dbModel);
        }

        public void Delete(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListDTO>(dto);
            var dbModel = GetModels(source => source.ModelId == model.ModelId, false).FirstOrDefault();
            if (dbModel == null) throw new NullReferenceException("ShoppingListDTO wasn't found in the database when trying to delete.");

            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListRepository.Delete(dbModel);
        }
    }
}