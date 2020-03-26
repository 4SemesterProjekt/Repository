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
        void AddShoppingList(ShoppingListDTO shoppingList);
        Task DeleteShoppingList(ShoppingListDTO shoppingList);
        void UpdateShoppingList(ShoppingListDTO shoppingList);
        Task<List<ItemDTO>> GetShoppingListByID(int ID);
        Task<List<ShoppingListDTO>> GetShoppingListsByGroupID(int GroupID);
        
        // Consider adding interface to criteria
    }

    public class ShoppingListRepository //: IShoppingListRepository
    {
        SplitListContext context;

        public ShoppingListRepository(SplitListContext Context)
        {
            context = Context;
        }

        public void DeleteShoppingList(ShoppingListDTO shoppingList)
        {

            ShoppingList list = LoadToModel(shoppingList);

            if ( list != null)
            {
                context.ShoppingLists.Remove(list);

                if (shoppingList.Items != null)
                {
                    foreach (ItemDTO item in shoppingList.Items)
                    {
                        ShoppingListItem ItemInList = context.ShoppingListItems.Find(item.ItemID);
                        if (ItemInList != null)
                        {
                            context.ShoppingListItems.Remove(ItemInList);
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        public void UpdateShoppingList(ShoppingListDTO shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);
            if ( list != null)
            {
                if (shoppingList.shoppingListName != list.Name)
                {
                    list.Name = shoppingList.shoppingListName;
                    context.ShoppingLists.Update(list);
                    context.SaveChanges();
                }
                
                foreach (ItemDTO item in shoppingList.Items)
                {
                    context.Items.Update(new Item
                    {
                        ItemID = item.ItemID,
                        Name = item.Name,
                        Type = item.Type
                    });

                    context.ShoppingListItems.Update(new ShoppingListItem
                    {
                        ShoppingListID = shoppingList.shoppingListID,
                        ItemID = item.ItemID,
                        Amount = item.Amount
                    });
                }
                context.SaveChanges();
            }
            
        }

        public List<ShoppingListDTO> GetShoppingLists()
        {
            List<ShoppingListDTO> repoList = new List<ShoppingListDTO>();
            var contextLists =  context.ShoppingLists
                .Include(a => a.Group)
                .ToList();

            foreach (ShoppingList list in contextLists)
            {
                repoList.Add( new ShoppingListDTO()
                {
                    shoppingListID = list.ShoppingListID,
                    shoppingListGroupID = list.GroupID,
                    shoppingListGroupName = list.Group.Name,
                    shoppingListName = list.Name
                });
            }
            return repoList;
        }

        public List<ShoppingListDTO> GetShoppingListsByGroupID(int GroupID)
        {
            List<ShoppingListDTO> repoList = new List<ShoppingListDTO>();
            var contextLists = context.ShoppingLists
                .Include(g => g.Group)
                .Where(a => a.GroupID == GroupID)
                .ToList();

            foreach (ShoppingList list in contextLists)
            {
                repoList.Add(new ShoppingListDTO()
                {
                    shoppingListID = list.ShoppingListID,
                    shoppingListGroupID = list.GroupID,
                    shoppingListGroupName = list.Group.Name,
                    shoppingListName = list.Name
                });
            }
            return repoList;
        }

        public ShoppingListDTO GetShoppingListByID(int ID)
        {
            ShoppingList dbList = context.ShoppingLists.Find(ID);
            if (dbList != null)
            {

                ShoppingListDTO list = new ShoppingListDTO()
                {
                    shoppingListID = dbList.ShoppingListID,
                    shoppingListGroupID = dbList.GroupID,
                    shoppingListName = dbList.Name
                };
                //Query doesnt work :)
                list.Items = context.ShoppingLists
                    .Where(sl => sl.ShoppingListID == ID)
                    .SelectMany(it => it.ShoppingListItems)
                    .Join(
                        context.Items,
                        sli => sli.ShoppingListID,
                        i => i.ItemID,
                        (sli, i) => new ItemDTO()
                        {
                            ItemID = sli.ItemID,
                            Name = sli.Item.Name,
                            Amount = sli.Amount,
                            Type = sli.Item.Type
                        }
                    ).ToList();

                list.shoppingListGroupName = context.Groups.Find(list.shoppingListGroupID).Name;
                return list;
            }
            else return null;
        }

        public ShoppingList LoadToModel(ShoppingListDTO shoppingList)
        {
            if (context.Groups.Find(shoppingList.shoppingListGroupID) != null)
            {
                ShoppingList list = context.ShoppingLists.Find(shoppingList.shoppingListID);

                if (list != null)
                    return list;
                else
                {
                    ShoppingList newList = new ShoppingList()
                    {
                        Name = shoppingList.shoppingListName,
                        GroupID = shoppingList.shoppingListGroupID,
                    };
                    context.ShoppingLists.Add(newList);
                    context.SaveChanges();
                    return newList;
                }
            }
            return null;
        }
    }
}