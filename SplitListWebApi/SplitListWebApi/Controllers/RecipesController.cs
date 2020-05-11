using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiFormat.Recipe;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services;
using SplitListWebApi.Services.Interfaces;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IPublicService<RecipeDTO, RecipeModel> _recipeService;
        public RecipesController(SplitListContext context, IMapper mapper)
        {
            _recipeService = new RecipeService(context, mapper);
        }

        // GET: api/Recipes
        [HttpGet]
        public List<RecipeDTO> GetAll()
        {
            return _recipeService.GetAll();
        }

        // POST: api/Recipes
        [HttpPost]
        public RecipeDTO Post([FromBody] RecipeDTO dto)
        {
            return _recipeService.Create(dto);
        }

        // PUT: api/Recipes/5
        [HttpPut]
        public RecipeDTO Put([FromBody] RecipeDTO dto)
        {
            return _recipeService.Update(dto);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete]
        public ActionResult Delete(RecipeDTO dto)
        {
            try
            {
                _recipeService.Delete(dto);
                return NoContent();
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
