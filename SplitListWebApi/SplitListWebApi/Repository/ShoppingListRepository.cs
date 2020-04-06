using System.Collections.Generic;
using System.Linq;
using SplitListWebApi.Models;
using ApiFormat;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.Features;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;

namespace SplitListWebApi.Repository
{
    public interface IShoppingListRepository
    {
        void DeleteShoppingList(ShoppingListDTO shoppingList);
        ShoppingListDTO UpdateShoppingList(ShoppingListDTO shoppingList);
        List<ShoppingListDTO> GetShoppingListsByGroupID(int GroupID);
        ShoppingListDTO GetShoppingListByID(int ID);
    }

    public class ShoppingListRepository : IShoppingListRepository
    {
        private SplitListContext context;
        private IItemRepository itemRepo;

        public ShoppingListRepository(SplitListContext Context)
        {
            context = Context;
            itemRepo = new ItemRepository(context);
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
            context.SaveChanges();
        }

        private void AddItemsToShoppingList(ShoppingListDTO shoppingList)
        {
            foreach (ItemDTO item in shoppingList.Items)
            {
                Item itemModel = itemRepo.LoadToModel(item);
                if (item.ItemID <= 0)
                    item.ItemID = itemModel.ItemID;

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

        public ShoppingListDTO UpdateShoppingList(ShoppingListDTO shoppingList)
        {
            ShoppingList list = LoadToModel(shoppingList);

            if (list != null)
            {
                if (shoppingList.shoppingListID <= 0)
                {
                    shoppingList.shoppingListID = list.ShoppingListID;
                }

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
            return shoppingList;
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