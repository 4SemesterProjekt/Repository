using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace SplitListWebApi.Repository
{
    public interface IShoppingListRepository
    {
        void AddShoppingList(ShoppingListFormat shoppingList);
        void DeleteShoppingList(ShoppingListFormat shoppingList);
        void UpdateShoppingList(ShoppingListFormat shoppingList);
        Task<List<ItemFormat>> GetShoppingListByID(int ID);
        Task<List<ShoppingListFormat>> GetShoppingListsByGroupID(int GroupID);
        
        // Consider adding interface to criteria
    }

    public class ShoppingListRepository : IShoppingListRepository
    {
        SplitListContext context;

        public ShoppingListRepository(SplitListContext Context)
        {
            context = Context;
        }

        public async void AddShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = await LoadToModel(shoppingList);
            context.ShoppingLists.Add(list);
            context.SaveChanges();
        }

        public async void DeleteShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = await LoadToModel(shoppingList);
            if (await context.ShoppingLists.FindAsync(list.ShoppingListID) != null)
            {
                context.ShoppingLists.Remove(list);
            }
            await context.SaveChangesAsync();
        }

        public async void UpdateShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = await LoadToModel(shoppingList);
            if (await context.ShoppingLists.FindAsync(list.ShoppingListID) == null)
            {
                AddShoppingList(shoppingList);
            }
            else
            {
               context.ShoppingLists.Update(list);
            }
        }

        public async Task<List<ShoppingListFormat>> GetShoppingLists()
        {
            List<ShoppingListFormat> repoList = new List<ShoppingListFormat>();
            var contextLists = await context.ShoppingLists
                .Include(a => a.Group)
                .ToListAsync();

            foreach (ShoppingList list in contextLists)
            {
                repoList.Add( new ShoppingListFormat()
                {
                    shoppingListID = list.ShoppingListID,
                    shoppingListGroupID = list.GroupID,
                    shoppingListGroupName = list.Group.Name,
                    shoppingListName = list.Name
                });
            }
            return repoList;
        }

        public async Task<List<ShoppingListFormat>> GetShoppingListsByGroupID(int GroupID)
        {
            List<ShoppingListFormat> repoList = new List<ShoppingListFormat>();
            var contextLists = await context.ShoppingLists
                .Include(g => g.Group)
                .Where(a => a.GroupID == GroupID)
                .ToListAsync();

            foreach (ShoppingList list in contextLists)
            {
                repoList.Add(new ShoppingListFormat()
                {
                    shoppingListID = list.ShoppingListID,
                    shoppingListGroupID = list.GroupID,
                    shoppingListGroupName = list.Group.Name,
                    shoppingListName = list.Name
                });
            }
            return repoList;
        }

        public async Task<List<ItemFormat>> GetShoppingListByID(int ID)
        {
            return await context.ShoppingLists
                .Where(sl => sl.ShoppingListID == ID)
                .SelectMany(it => it.ShoppingListItems)
                .Join(
                    context.Items,
                    sli => sli.ShoppingListID,
                    i => i.ItemID,
                    (sli, i) => new ItemFormat()
                    {
                        ItemID = sli.ItemID,
                        Name = sli.Item.Name,
                        Amount = sli.Amount,
                        Type = sli.Item.Type
                    }
                ).ToListAsync();
        }

        public async Task<ShoppingList> LoadToModel(ShoppingListFormat shoppingList)
        {
            return new ShoppingList()
            {
                Name = shoppingList.shoppingListName,
                Group = await context.Groups.FindAsync(shoppingList.shoppingListGroupID),
                GroupID = shoppingList.shoppingListGroupID,
                ShoppingListID = shoppingList.shoppingListID,
                ShoppingListItems = new List<ShoppingListItem>()
            };
        }
    }
}