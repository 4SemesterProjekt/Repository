using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using ApiFormat;
using ApiFormat.ShoppingList;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Services.Interfaces;
using SplitListWebApi.Services;
using AutoMapper;
using System;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly SplitListContext _context;
        private IService<ShoppingListDTO, int> service;

        public ShoppingListsController(SplitListContext context, IMapper mapper)
        {
            _context = context;
            service = new ShoppingListService(context, mapper);
        }

        // Returns ShoppingListDTO object for a specific shoppinglist ID
        [HttpGet("{id}")]
        public ShoppingListDTO GetShoppingListByID(int id)
        {
            return service.GetById(id);
        }

        // POST: api/ShoppingLists
        [HttpPost]
        public ShoppingListDTO Create(ShoppingListDTO dto)
        {
            return service.Create(dto);
        }

        [HttpPut]
        public ShoppingListDTO Update(ShoppingListDTO dto)
        {
            return service.Update(dto);
        }

        // DELETE: api/ShoppingLists
        [HttpDelete]
        public IActionResult DeleteShoppingList(ShoppingListDTO dto)
        {
            try
            {
                service.Delete(dto);
                return Ok(dto);
            }
            catch(Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}
