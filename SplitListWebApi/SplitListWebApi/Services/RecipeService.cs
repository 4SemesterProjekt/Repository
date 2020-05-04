using System;
using System.Collections.Generic;
using System.Linq;
using ApiFormat.Item;
using ApiFormat.Recipe;
using AutoMapper;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Services
{
    public class RecipeService : IService<RecipeDTO, int>
    {
        private IMapper _mapper;
        private GenericRepository<RecipeModel> _recipeRepository;
        private RecipeItemRepository _repiRepo;
        private IService<ItemDTO, int> _itemService;

        public RecipeService(SplitListContext context, IMapper mapper)
        {
            _mapper = mapper;
            _itemService = new ItemService(context, mapper);
            _recipeRepository = new GenericRepository<RecipeModel>(context);
            _repiRepo = new RecipeItemRepository(context);
        }

        public List<RecipeDTO> GetAll()
        {
            return _mapper.Map<List<RecipeDTO>>(_recipeRepository.GetBy(
                selector: source => source,
                predicate: source => true,
                include: source => source
                    .Include(rm => rm.RecipeItems)
                    .ThenInclude(ri => ri.ItemModel)));
        }

        public RecipeDTO Create(RecipeDTO dto)
        {
            var model = _mapper.Map<RecipeModel>(dto);
            var dbModel = _recipeRepository.GetBy(
                selector: source => source,
                predicate: rm => rm.ModelId == model.ModelId,
                include: source => source
                    .Include(rm => rm.RecipeItems)
                        .ThenInclude(ri => ri.ItemModel),
                disableTracking: false)
                .FirstOrDefault();

            if (dbModel != null) return _mapper.Map<RecipeDTO>(dbModel);

            dbModel = _recipeRepository.Create(model);

            if (dto.Items != null)
            {
                for (int i = 0; i < dto.Items.Count; i++)
                { 
                    dto.Items[i] = _itemService.Create(dto.Items[i]);
                }

                _repiRepo.CreateRecipeItems(dto.Items, dbModel);
            }

            var tempModel = dbModel;

            dbModel = _recipeRepository.GetBy(
                selector: source => source,
                predicate: rm => rm.ModelId == tempModel.ModelId,
                include: source => source
                    .Include(rm => rm.RecipeItems)
                        .ThenInclude(ri => ri.ItemModel))
                .FirstOrDefault();

            return _mapper.Map<RecipeDTO>(dbModel);
        }

        public RecipeDTO Update(RecipeDTO dto)
        {
            var model = _mapper.Map<RecipeModel>(dto);
            var dbModel = _recipeRepository.GetBy(
                selector: source => source,
                predicate: rm => rm.ModelId == model.ModelId,
                include: source => source
                    .Include(rm => rm.RecipeItems)
                    .ThenInclude(ri => ri.ItemModel),
                disableTracking: false)
                .FirstOrDefault();

            if (dbModel == null)
                return _mapper.Map<RecipeDTO>(_recipeRepository.Create(model));

            dbModel.Name = dto.Name;
            dbModel.Introduction = dto.Introduction;
            dbModel.Method = dto.Method;
            _recipeRepository.Update(dbModel);

            if (dto.Items != null) 
            {
                for (int i = 0; i < dto.Items.Count; i++)
                {
                    dto.Items[i] = _itemService.Create(dto.Items[i]);
                }
            }
            _repiRepo.DeleteRecipeItems(dbModel.RecipeItems);
            _repiRepo.CreateRecipeItems(dto.Items, dbModel);
            return _mapper.Map<RecipeDTO>(dbModel);
        }

        public void Delete(RecipeDTO dto)
        {
            var model = _mapper.Map<RecipeModel>(dto);
            var dbModel = _recipeRepository.GetBy(
                selector: source => source,
                predicate: rm => rm.ModelId == model.ModelId,
                include: source => source
                    .Include(rm => rm.RecipeItems)
                    .ThenInclude(ri => ri.ItemModel),
                disableTracking: false)
                .FirstOrDefault();

            if (dbModel == null) throw new NullReferenceException("RecipeDTO wasn't found in the database when trying to delete.");

            _repiRepo.DeleteRecipeItems(dbModel.RecipeItems);
            _recipeRepository.Delete(dbModel);
        }
    }
}