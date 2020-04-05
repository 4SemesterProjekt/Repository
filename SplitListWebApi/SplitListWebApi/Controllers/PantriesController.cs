using System.Collections.Generic;
using ApiFormat;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PantriesController : ControllerBase
    {
        private readonly SplitListContext _context;
        private IPantryRepository repo;

        public PantriesController(SplitListContext context)
        {
            _context = context;
            repo = new PantryRepository(context);
        }

        // GET: api/Pantries/group/5
        // Returns pantry for a specific group
        [HttpGet("group/{id}")]
        public PantryDTO GetPantryByGroupID(int id)
        {
            return repo.GetPantryFromGroupID(id);
        }

        // POST: api/Pantries
        // Updates/Creates pantry from parameter
        [HttpPost]
        public PantryDTO PostShoppingList(PantryDTO pantryDto)
        {
            return repo.UpdatePantry(pantryDto);
        }

        // DELETE: api/Pantries
        // Deletes pantry from parameter
        [HttpDelete]
        public void DeleteShoppingList(PantryDTO pantryDto)
        {
            repo.DeletePantry(pantryDto);
        }
    }
}