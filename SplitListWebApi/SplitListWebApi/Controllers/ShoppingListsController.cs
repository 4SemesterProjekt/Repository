using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.WebEncoders.Testing;
using Newtonsoft.Json;
using SplitListWebApi.Models;

namespace SplitListWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly SplitListContext _context;

        public ShoppingListsController(SplitListContext context)
        {
            _context = context;
        }

        // GET: api/ShoppingLists
        //Task<ActionResult<IEnumerable<ShoppingList>>
        [HttpGet]
        public /*async*/ Task<object> GetShoppingLists()
        {
            /*var shoppingLists = await _context.ShoppingLists
                .Select(sl => new ShoppingListDBF()
                {
                    shoppingListID = sl.ShoppingListID,
                    shoppingListName = sl.Name,
                    shoppingListGroupID = sl.GroupID,
                    shoppingListGroupName = sl.Group.Name
                }).ToListAsync();

            return shoppingLists;*/
            return null;
        }

        // Returns all shopping lists for a specific group
        // Task<ActionResult<ShoppingList>>
        [HttpGet("group/{id}")]
        public async Task<object> GetShoppingList(int id)
        {
            var shoppingLists = await _context.ShoppingLists
                .Where(sl => sl.GroupID == id)
                .ToListAsync();

            if (shoppingLists == null)
            {
                return NotFound();
            }

            return shoppingLists.Join(
                _context.Groups,
                sl => sl.GroupID,
                g => g.GroupID,
                (sl, g) => new
                {
                    ShoppingListName = sl.Name,
                    GroupName = g.Name
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<object> GetShoppingLists(int id)
        {
            /*_context.Update(_context.ShoppingLists
                .Where(sl => sl.ShoppingListID == id)
                .SelectMany(it => it.ShoppingListItems)
                .Join(
                    _context.Items,
                    sli => sli.ShoppingListID,
                    i => i.ItemID,
                    (sli, i) => new
                    {
                        ItemName = sli.Item.Name,
                        Quantity = sli.Amount
                    }
                ).ToList());*/
            return await _context.ShoppingLists
                .Where(sl => sl.ShoppingListID == id)
                .SelectMany(it => it.ShoppingListItems)
                .Join(
                    _context.Items,
                    sli => sli.ShoppingListID,
                    i => i.ItemID,
                    (sli, i) => new
                    {
                        ItemName = sli.Item.Name,
                        Quantity = sli.Amount
                    }
                ).ToListAsync();
        }

        // PUT: api/ShoppingLists/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutShoppingList(int id, ShoppingList shoppingList)
        {
            if (id != shoppingList.ShoppingListID)
            {
                return BadRequest();
            }

            _context.Entry(shoppingList).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShoppingListExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
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
        public async Task<ActionResult<ShoppingList>> DeleteShoppingList(int id)
        {
            var shoppingList = await _context.ShoppingLists.FindAsync(id);
            if (shoppingList == null)
            {
                return NotFound();
            }

            _context.ShoppingLists.Remove(shoppingList);
            await _context.SaveChangesAsync();

            return shoppingList;
        }

        private bool ShoppingListExists(int id)
        {
            return _context.ShoppingLists.Any(e => e.ShoppingListID == id);
        }
    }
}
