using System.Collections.Generic;
using System.Linq;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class PantryService : IService<PantryDTO, int>
    {
        private SplitListContext _context;
        private readonly IMapper _mapper;
        private readonly GenericRepository<PantryModel> _pantryRepository;
        private readonly GenericRepository<GroupModel> _groupRepository;
        private readonly PantryItemRepository _pantryItemRepo;
        private readonly IService<ItemDTO, int> _itemService;
        public PantryService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _pantryRepository = new GenericRepository<PantryModel>(context);
            _groupRepository = new GenericRepository<GroupModel>(context);
            _pantryItemRepo = new PantryItemRepository(context);
            _itemService = new ItemService(context, mapper);
        }


        public PantryDTO GetById(int id)
        {
            return _mapper.Map<PantryDTO>(_pantryRepository.GetBy(
                selector: source => source,
                predicate: pm => pm.ModelId == id,
                include: source =>
                    source.Include(pm => pm.PantryItems)
                            .ThenInclude(pi => pi.ItemModel)
                        .Include(pm => pm.GroupModel),
                disableTracking: false));
        }

        public PantryDTO Create(PantryDTO dto)
        {
            var model = _mapper.Map<PantryModel>(dto);
            var dbModel = _pantryRepository.GetBy(
                selector: source => source,
                predicate: pm => pm.ModelId == model.ModelId,
                include: source =>
                    source.Include(pm => pm.PantryItems)
                            .ThenInclude(pi => pi.ItemModel)
                        .Include(pm => pm.GroupModel),
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

                dbModel = _pantryRepository.Create(model);

                if (dto.Items != null)
                {
                    for (int i = 0; i < dto.Items.Count; i++)
                    {
                        dto.Items[i] = _itemService.Create(dto.Items[i]);
                    }

                    _pantryItemRepo.CreatePantryItems(dto.Items, dbModel);
                }

                dbModel = _pantryRepository.GetBy(
                    selector: source => source,
                    predicate: pm => pm.ModelId == model.ModelId,
                    include: source =>
                        source.Include(pm => pm.PantryItems)
                            .ThenInclude(pi => pi.ItemModel)
                            .Include(pm => pm.GroupModel),
                    disableTracking: false);

                return _mapper.Map<PantryDTO>(dbModel);
            }
            return _mapper.Map<PantryDTO>(dbModel);
        }

        public PantryDTO Update(PantryDTO dto)
        {
            var model = _mapper.Map<PantryModel>(dto);
            var dbModel = _pantryRepository.GetBy(
                selector: source => source,
                predicate: pm => pm.ModelId == model.ModelId,
                include: source =>
                    source.Include(pm => pm.PantryItems)
                        .ThenInclude(pi => pi.ItemModel)
                        .Include(pm => pm.GroupModel),
                disableTracking: false);

            if (dbModel == null)
                return _mapper.Map<PantryDTO>(_pantryRepository.Create(model));

            dbModel.Name = dto.Name;

            if (dto.Items != null)
            {
                for (int i = 0; i < dto.Items.Count; i++)
                {
                    dto.Items[i] = _itemService.Update(dto.Items[i]);
                }
            }
            _pantryItemRepo.DeletePantryItems(dbModel.PantryItems);
            _pantryItemRepo.CreatePantryItems(dto.Items, dbModel);
            return _mapper.Map<PantryDTO>(dbModel);
        }

        public void Delete(PantryDTO dto)
        {
            var model = _mapper.Map<PantryDTO>(dto);
            var dbModel = _pantryRepository.GetBy(
                selector: source => source,
                predicate: pm => pm.ModelId == model.ModelId,
                include: source =>
                    source.Include(pm => pm.PantryItems)
                        .ThenInclude(pi => pi.ItemModel),
                disableTracking: false);
            if (dbModel == null) return;

            _pantryItemRepo.DeletePantryItems(dbModel.PantryItems);
            _pantryRepository.Delete(dbModel);
        }
    }
}
