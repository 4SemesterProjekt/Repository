using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;

namespace SplitListWebApi.Repository
{
    public interface IShoppingListRepository
    {
        void AddShoppingList(ShoppingListFormat shoppingList);
        ShoppingList FindShoppingList(int id);
        void DeleteShoppingList(ShoppingListFormat shoppingList);
        void UpdateShoppingList(ShoppingListFormat shoppingList);
        //List<ShoppingListFormat> GetShoppingLists();
        // Add interface to criteria
    }

    public class ShoppingListRepository : IShoppingListRepository
    {
        SplitListContext context;

        public ShoppingListRepository(SplitListContext Context)
        {
            context = Context;
        }

        public void AddShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);
            context.ShoppingLists.Add(list);
            context.SaveChanges();
        }

        public ShoppingList FindShoppingList(int id)
        {
            return context.ShoppingLists.Find(id);
        }

        public void DeleteShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);
            if (context.ShoppingLists.Find(list.ShoppingListID) != null)
            {
                context.ShoppingLists.Remove(list);
            }
            context.SaveChanges();
        }

        public void UpdateShoppingList(ShoppingListFormat shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);
            if (context.ShoppingLists.Find(list.ShoppingListID) == null)
            {
                AddShoppingList(shoppingList);
            }
            else
            {
                context.ShoppingLists.Update(list);
            }
        }

        public ShoppingListFormat GetShoppingLists()
        {
            /*List<ShoppingListFormat> repoList = new List<ShoppingListFormat>();
            var contextLists = context.ShoppingLists.ToList();
            foreach (ShoppingList list in contextLists)
            {
                var temp = new ShoppingListFormat()
                {
                    shoppingListID = list.ShoppingListID,
                    shoppingListGroupID = list.GroupID,
                    shoppingListGroupName = list.Group.Name,
                    shoppingListName = list.Name
                };
                repoList.Add(temp);
            }*/
            var contextLists = context.ShoppingLists.FirstOrDefault();
            var repoList = new ShoppingListFormat()
            {
                shoppingListID = contextLists.ShoppingListID,
                shoppingListName = contextLists.Name,
                shoppingListGroupID = contextLists.GroupID,
                shoppingListGroupName = contextLists.Group.Name
            };

            return repoList;
        }

        public ShoppingList LoadToModel(ShoppingListFormat shoppingList)
        {
            return new Models.ShoppingList()
            {
                Name = shoppingList.shoppingListName,
                Group = context.Groups.Find(shoppingList.shoppingListGroupID),
                GroupID = shoppingList.shoppingListGroupID,
                ShoppingListID = shoppingList.shoppingListID,
                ShoppingListItems = new List<ShoppingListItem>()
            };
        }

    }
}