using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
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
        private readonly GenericRepository<GroupModel> _groupRepository;
        private readonly ShoppingListItemRepository _shoppingListItemRepo;

        public ShoppingListService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _shoppingListRepository = new GenericRepository<ShoppingListModel>(context);
            _groupRepository = new GenericRepository<GroupModel>(context);
            _shoppingListItemRepo = new ShoppingListItemRepository(context);
        }


        public ShoppingListDTO GetById(int id)
        {
            return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.GetBy(
                selector: source => source,
                predicate: slm => slm.ModelId == id,
                include: source =>
                    source.Include(slm => slm.ShoppingListItems)
                        .ThenInclude(sli => sli.ItemModel)));
        }

        public ShoppingListDTO Create(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = _shoppingListRepository.GetBy(
                selector: source => source,
                predicate: slm => slm.ModelId == model.ModelId,
                include: source =>
                    source.Include(slm => slm.ShoppingListItems)
                            .ThenInclude(sli => sli.ItemModel)
                        .Include(slm => slm.GroupModel),
                disableTracking: false);

            if (dbModel == null)
            {
                if (dto.Group.ModelId != 0)
                {
                    model.GroupModel = _groupRepository.GetBy(
                        selector: source => source,
                        predicate: gm => gm.ModelId == dto.Group.ModelId,
                        disableTracking: false
                    );
                }
                dbModel = _shoppingListRepository.Create(model);
                var itemModels = _mapper.Map<List<ItemModel>>(dto.Items);
                _shoppingListItemRepo.CreateShoppingListItems(itemModels, dbModel);

                dbModel = _shoppingListRepository.GetBy(
                    selector: source => source,
                    predicate: slm => slm.ModelId == model.ModelId,
                    include: source =>
                        source.Include(slm => slm.ShoppingListItems)
                            .ThenInclude(sli => sli.ItemModel)
                            .Include(slm => slm.GroupModel),
                    disableTracking: false);

                //dbModel.ShoppingListItems = _shoppingListItemRepo.GetBy(dbModel.ModelId, itemModels.Select(im => im.ModelId));
                return _mapper.Map<ShoppingListDTO>(dbModel);
            }

            return _mapper.Map<ShoppingListDTO>(dbModel);
        }

        public ShoppingListDTO Update(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListModel>(dto);
            var dbModel = _shoppingListRepository.GetBy(
                selector: source => source,
                predicate: slm => slm.ModelId == model.ModelId,
                include: source =>
                    source.Include(slm => slm.ShoppingListItems)
                        .ThenInclude(sli => sli.ItemModel)
                        .Include(slm => slm.GroupModel),
                disableTracking: false);

            if (dbModel == null)
                return _mapper.Map<ShoppingListDTO>(_shoppingListRepository.Create(model));

            dbModel.Name = dto.Name;

            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListItemRepo.CreateShoppingListItems(_mapper.Map<List<ItemModel>>(dto.Items), dbModel);
            return _mapper.Map<ShoppingListDTO>(dbModel);
        }

        public void Delete(ShoppingListDTO dto)
        {
            var model = _mapper.Map<ShoppingListDTO>(dto);
            var dbModel = _shoppingListRepository.GetBy(
                selector: source => source,
                predicate: slm => slm.ModelId == model.ModelId,
                include: source =>
                    source.Include(slm => slm.ShoppingListItems)
                        .ThenInclude(sli => sli.ItemModel),
                disableTracking: false);
            if (dbModel == null) return;

            _shoppingListItemRepo.DeleteShoppingListItems(dbModel.ShoppingListItems);
            _shoppingListRepository.Delete(dbModel);
        }
    }
}