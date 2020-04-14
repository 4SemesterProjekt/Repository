//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore.Internal;
//using SplitListWebApi.Repository;
//using ApiFormat;
//using ApiFormat.ShoppingList;
//using SplitListWebApi.Areas.Identity.Data;

//namespace SplitListWebApi.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class ShoppingListsController : ControllerBase
//    {
//        private readonly SplitListContext _context;
//        private IShoppingListRepository repo;

//        public ShoppingListsController(SplitListContext context)
//        {
//            _context = context;
//            repo = new ShoppingListRepository(context);
//        }

//        // Returns all shopping lists for a specific group
//        [HttpGet("group/{id}")]
//        public List<ShoppingListDTO> GetShoppingListsByGroupID(int id)
//        {
//            return repo.GetShoppingListsByGroupID(id);
//        }

//        // Returns ShoppingListDTO object for a specific shoppinglist ID
//        [HttpGet("{id}")]
//        public ShoppingListDTO GetShoppingListByID(int id)
//        {
//            return repo.GetShoppingListByID(id);
//        }

//        // POST: api/ShoppingLists
//        // Updates/Creates shoppinglist from parameter.
//        [HttpPost]
//        public ShoppingListDTO PostShoppingList(ShoppingListDTO shoppingList)
//        {
//            return repo.UpdateShoppingList(shoppingList);
//        }

//        // DELETE: api/ShoppingLists
//        [HttpDelete]
//        public void DeleteShoppingList(ShoppingListDTO list)
//        {
//            repo.DeleteShoppingList(list);
//        }
//    }
//}
