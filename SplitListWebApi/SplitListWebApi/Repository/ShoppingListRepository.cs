using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;

namespace SplitListWebApi.Repository
{
    public interface IShoppingListRepository
    {
        void DeleteShoppingList(ShoppingListDTO shoppingList);
        void UpdateShoppingList(ShoppingListDTO shoppingList);
        List<ShoppingListDTO> GetShoppingListsByGroupID(int GroupID);
        ShoppingListDTO GetShoppingListByID(int ID);
    }

    public class ShoppingListRepository : IShoppingListRepository
    {
        private SplitListContext context;

        public ShoppingListRepository(SplitListContext Context)
        {
            context = Context;
        }

        public void DeleteShoppingList(ShoppingListDTO shoppingList)
        {
            // Missing validation for OwnerID. Only owner should be able to delete shoppinglist.
            ShoppingList list = LoadToModel(shoppingList);

            if (list != null)
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

        private void RemoveItemsFromShoppingList(ShoppingListDTO shoppingList)
        {
            List<ShoppingListItem> dbItemsInSL = context.ShoppingListItems
                .Where(sli => sli.ShoppingListID == shoppingList.shoppingListID)
                .Include(it => it.Item)
                .ToList();

            foreach (ShoppingListItem slItem in dbItemsInSL)
            {
                ItemDTO itemToRemove = shoppingList.Items.Find(it => it.ItemID == slItem.ItemID);
                if (itemToRemove == null)
                {
                    context.ShoppingListItems.Remove(slItem);
                }
            }
        }

        private void AddItemsToShoppingList(ShoppingListDTO shoppingList)
        {
            foreach (ItemDTO item in shoppingList.Items)
            {
                Item itemModel = context.Items.Find(item.ItemID);
                if (itemModel != null)
                {
                    itemModel.Name = item.Name;
                    itemModel.Type = item.Type;
                    context.Items.Update(itemModel);
                    context.SaveChanges();
                }
                else
                {
                    context.Items.Add(new Item()
                    {
                        ItemID = item.ItemID,
                        Name = item.Name,
                        Type = item.Type
                    });
                    context.SaveChanges();
                }

                ShoppingListItem shoppingListItemModel = context.ShoppingListItems.Find(shoppingList.shoppingListID, item.ItemID);
                if (shoppingListItemModel != null)
                {
                    shoppingListItemModel.Amount = item.Amount;
                    context.Update(shoppingListItemModel);
                    context.SaveChanges();
                }
                else
                {
                    context.ShoppingListItems.Add(new ShoppingListItem()
                    {
                        ItemID = item.ItemID,
                        Amount = item.Amount,
                        ShoppingListID = shoppingList.shoppingListID
                    });
                    context.SaveChanges();
                }
            }
        }

        public void UpdateShoppingList(ShoppingListDTO shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);
            if (list != null)
            {
                list.Name = shoppingList.shoppingListName;
                context.ShoppingLists.Update(list);
                context.SaveChanges();

                if (shoppingList.Items == null)
                {
                    shoppingList.Items = new List<ItemDTO>();
                }
                else
                {
                    RemoveItemsFromShoppingList(shoppingList);
                    AddItemsToShoppingList(shoppingList);
                }
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
                    shoppingListName = dbList.Name,
                    Items = new List<ItemDTO>()
                };
                
                List<ShoppingListItem> shoppingListItems = context.ShoppingLists
                    .Where(sl => sl.ShoppingListID == ID)
                    .Include(sli => sli.ShoppingListItems)
                    .ThenInclude(it => it.Item)
                    .SelectMany(sl => sl.ShoppingListItems)
                    .ToList();
                if (shoppingListItems != null)
                {
                    foreach (var item in shoppingListItems)
                    {
                        list.Items.Add(new ItemDTO()
                        {
                            Amount = item.Amount,
                            ItemID = item.ItemID,
                            Name = item.Item.Name,
                            Type = item.Item.Type
                        });
                    }
                }

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