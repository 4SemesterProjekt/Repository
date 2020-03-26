using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using SplitListWebApi.Models;
using SplitListWebApi.Repository;
using ApiFormat;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly SplitListContext _context;
        private IShoppingListRepository repo;

        public ShoppingListsController(SplitListContext context)
        {
            _context = context;
            repo = new ShoppingListRepository(context);
        }

        // Returns all shopping lists for a specific group
        [HttpGet("group/{id}")]
        public List<ShoppingListDTO> GetShoppingList(int id)
        {
            return repo.GetShoppingListsByGroupID(id);
        }

        //Returns all items in specific shoppinglist
        [HttpGet("{id}")]
        public ShoppingListDTO GetShoppingLists(int id)
        {
            return repo.GetShoppingListByID(id);
        }
         
        // PUT: api/ShoppingLists/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public void PutShoppingList(int id)
        {
            ShoppingListDTO list = repo.GetShoppingListByID(id);
            repo.UpdateShoppingList(list);
        }

        // POST: api/ShoppingLists
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<ShoppingList>> PostShoppingList(ShoppingList shoppingList)
        {
            _context.ShoppingLists.Add(shoppingList);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShoppingList", new { id = shoppingList.ShoppingListID }, shoppingList);
        }

        // DELETE: api/ShoppingLists/5
        [HttpDelete("{id}")]
        public void DeleteShoppingList(ShoppingListDTO list)
        {
            repo.DeleteShoppingList(list);
        }

        private bool ShoppingListExists(int id)
        {
            return _context.ShoppingLists.Any(e => e.ShoppingListID == id);
        }
    }
}
