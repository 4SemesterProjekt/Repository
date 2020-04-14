using System;
using ApiFormat;
using ApiFormat.Group;
using ApiFormat.ShoppingList;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Repositories.Implementation;
using SplitListWebApi.Utilities;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private SplitListContext _context;
        private GenericRepository<ShoppingListDTO, ShoppingListModel> _repository;

        public ShoppingListsController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            _repository = new GenericRepository<ShoppingListDTO, ShoppingListModel>(_context, mapper);
        }

        [HttpPost("Create")]
        public ShoppingListDTO Create([FromBody] ShoppingListDTO dto)
        {
            return dto.Add(_repository);
        }

        [HttpGet("{id}")]
        public ShoppingListDTO GetById(double id)
        {
            ShoppingListDTO dto = new ShoppingListDTO();
            return dto.GetById(_repository, id);
        }

        [HttpDelete("Delete")]
        public void Delete([FromBody] ShoppingListDTO dto)
        {
            dto.Delete(_repository);
        }

        [HttpPost("Save")]
        public ShoppingListDTO Save(ShoppingListDTO dto)
        {
            return dto.Save(_repository);
        }
    }
}

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
