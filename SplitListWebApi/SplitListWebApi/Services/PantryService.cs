using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.Item;
using ApiFormat.Pantry;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Repositories.Interfaces;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class PantryService : IPublicService<PantryDTO, PantryModel>, IModelService<PantryDTO, PantryModel>
    {
        private SplitListContext _context;
        private readonly IMapper _mapper;
        private readonly IRepository<PantryModel> _pantryRepository;
        private readonly IRepository<GroupModel> _groupRepository;
        private readonly PantryItemRepository _pantryItemRepo;
        private readonly IPublicService<ItemDTO, ItemModel> _itemService;
        public PantryService(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _pantryRepository = new GenericRepository<PantryModel>(context);
            _groupRepository = new GenericRepository<GroupModel>(context);
            _pantryItemRepo = new PantryItemRepository(context);
            _itemService = new ItemService(context, mapper);
        }

        public IEnumerable<PantryModel> GetModels(Expression<Func<PantryModel, bool>> predicate, bool disableTracking = true)
        {
            return _pantryRepository.GetBy(
                selector: source => source,
                predicate: predicate,
                include: source =>
                    source.Include(pm => pm.PantryItems)
                        .ThenInclude(pi => pi.ItemModel)
                        .Include(pm => pm.GroupModel),
                disableTracking: disableTracking);
        }

        public List<PantryDTO> GetAll()
        {
            throw new NotImplementedException();
        }

        public PantryDTO GetBy(Expression<Func<PantryModel, bool>> predicate)
        {
            return _mapper.Map<PantryDTO>(GetModels(predicate).FirstOrDefault());
        }

        public PantryDTO Create(PantryDTO dto)
        {
            var model = _mapper.Map<PantryModel>(dto);
            var dbModel = GetModels(pm => pm.ModelId == model.ModelId, false).FirstOrDefault();

            if (dbModel == null)
            {
                if (dto.Group.ModelId != 0)
                {
                    model.GroupModel = _groupRepository.GetBy(
                        selector: source => source,
                        predicate: gm => gm.ModelId == dto.Group.ModelId,
                        disableTracking: false)
                        .FirstOrDefault();
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

                dbModel = GetModels(pm => pm.ModelId == model.ModelId, false).FirstOrDefault();

                return _mapper.Map<PantryDTO>(dbModel);
            }
            return _mapper.Map<PantryDTO>(dbModel);
        }

        public PantryDTO Update(PantryDTO dto)
        {
            var model = _mapper.Map<PantryModel>(dto);
            var dbModel = GetModels(pm => pm.ModelId == model.ModelId, false).FirstOrDefault();

            if (dbModel == null) throw new ArgumentException("PantryModel to update wasn't found in the database.");

            dbModel.Name = dto.Name;
            _pantryRepository.Update(dbModel);

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
            var dbModel = GetModels(pm => pm.ModelId == model.ModelId, false).FirstOrDefault();
            if (dbModel == null) throw new NullReferenceException("PantryDTO wasn't found in the database when trying to delete.");

            _pantryItemRepo.DeletePantryItems(dbModel.PantryItems);
            _pantryRepository.Delete(dbModel);
        }
    }
}
