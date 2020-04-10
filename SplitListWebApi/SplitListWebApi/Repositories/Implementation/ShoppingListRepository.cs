//using System.Collections.Generic;
//using System.Linq;
//using ApiFormat;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using ApiFormat.Item;
//using ApiFormat.ShadowTables;
//using ApiFormat.ShoppingList;
//using Microsoft.AspNetCore.Http.Features;
//using SplitListWebApi.Areas.Identity.Data;

//namespace SplitListWebApi.Repository
//{
//    public interface IShoppingListRepository
//    {
//        void DeleteShoppingList(IShoppingListDTO shoppingList);
//        IShoppingListDTO UpdateShoppingList(IShoppingListDTO shoppingList);
//        List<IShoppingListDTO> GetShoppingListsByGroupID(int GroupID);
//        IShoppingListDTO GetShoppingListByID(int ID);
//    }

//    public class ShoppingListRepository : IShoppingListRepository
//    {
//        private SplitListContext context;
//        private IItemRepository itemRepo;

//        public ShoppingListRepository(SplitListContext Context)
//        {
//            context = Context;
//            itemRepo = new ItemRepository(context);
//        }

//        public void DeleteShoppingList(IShoppingListDTO shoppingList)
//        {
//            // Missing validation for OwnerID. Only owner should be able to delete shoppinglist.
//            ShoppingList list = LoadToModel(shoppingList);

//            if (list != null)
//            {
//                context.ShoppingLists.Remove(list);

//                if (shoppingList.Items != null)
//                {
//                    foreach (IItemDTO item in shoppingList.Items)
//                    {
//                        ShoppingListItem ItemInList = context.ShoppingListItems.Find(item.ItemID);
//                        if (ItemInList != null)
//                        {
//                            context.ShoppingListItems.Remove(ItemInList);
//                        }
//                    }
//                }
//            }
//            context.SaveChanges();
//        }

//        private void RemoveItemsFromShoppingList(IShoppingListDTO shoppingList)
//        {
//            List<ShoppingListItem> dbItemsInSL = context.ShoppingListItems
//                .Where(sli => sli.ShoppingListID == shoppingList.shoppingListID)
//                .Include(it => it.Item)
//                .ToList();

//            foreach (ShoppingListItem slItem in dbItemsInSL)
//            {
//                IItemDTO itemToRemove = shoppingList.Items.Find(it => it.ItemID == slItem.ItemID);
//                if (itemToRemove == null)
//                {
//                    context.ShoppingListItems.Remove(slItem);
//                }
//            }
//            context.SaveChanges();
//        }

//        private void AddItemsToShoppingList(IShoppingListDTO shoppingList)
//        {
//            foreach (IItemDTO item in shoppingList.Items)
//            {
//                Item itemModel = itemRepo.LoadToModel(item);
//                if (item.ItemID <= 0)
//                    item.ItemID = itemModel.ItemID;

//                ShoppingListItem shoppingListItemModel = context.ShoppingListItems.Find(shoppingList.shoppingListID, item.ItemID);
//                if (shoppingListItemModel != null)
//                {
//                    shoppingListItemModel.Amount = item.Amount;
//                    context.Update(shoppingListItemModel);
//                    context.SaveChanges();
//                }
//                else
//                {
//                    context.ShoppingListItems.Add(new ShoppingListItem()
//                    {
//                        ItemID = item.ItemID,
//                        Amount = item.Amount,
//                        ShoppingListID = shoppingList.shoppingListID
//                    });
//                    context.SaveChanges();
//                }
//            }
//        }

//        public IShoppingListDTO UpdateShoppingList(IShoppingListDTO shoppingList)
//        {
//            ShoppingList list = LoadToModel(shoppingList);

//            if (list != null)
//            {
//                if (shoppingList.shoppingListID <= 0)
//                {
//                    shoppingList.shoppingListID = list.ShoppingListID;
//                }

//                list.Name = shoppingList.shoppingListName;
//                context.ShoppingLists.Update(list);
//                context.SaveChanges();

//                if (shoppingList.Items == null)
//                {
//                    shoppingList.Items = new List<IItemDTO>();
//                }
//                else
//                {
//                    RemoveItemsFromShoppingList(shoppingList);
//                    AddItemsToShoppingList(shoppingList);
//                }
//            }
//            return shoppingList;
//        }

//        public List<IShoppingListDTO> GetShoppingListsByGroupID(int GroupID)
//        {
//            List<IShoppingListDTO> lists = new List<IShoppingListDTO>();

//            var contextLists = context.ShoppingLists
//                .Include(g => g.Group)
//                .Where(a => a.GroupID == GroupID)
//                .ToList();

//            if (contextLists != null)
//            {
//                foreach (ShoppingList list in contextLists)
//                {
//                    lists.Add(GenerateDTOFromModel(list));
//                }

//                return lists;
//            }
//            return null;
//        }

//        public IShoppingListDTO GetShoppingListByID(int ID)
//        {
//            ShoppingList dbList = context.ShoppingLists.Find(ID);
//            if (dbList != null)
//            {
//                IShoppingListDTO list = GenerateDTOFromModel(dbList);
//                list = AddItemsToDTO(list);
//                return list;
//            }
//            else return null;
//        }

//        private IShoppingListDTO AddItemsToDTO(IShoppingListDTO list)
//        {
//            List<ShoppingListItem> shoppingListItems = context.ShoppingLists
//                   .Where(sl => sl.ShoppingListID == list.shoppingListID)
//                   .Include(sli => sli.ShoppingListItems)
//                   .ThenInclude(it => it.Item)
//                   .SelectMany(sl => sl.ShoppingListItems)
//                   .ToList();

//            if (shoppingListItems != null)
//            {
//                foreach (ShoppingListItem item in shoppingListItems)
//                {
//                    list.Items.Add(new Item()
//                    {
//                        Amount = item.Amount,
//                        ItemID = item.ItemID,
//                        Name = item.Item.Name,
//                        Type = item.Item.Type
//                    });
//                }
//            }
//            return list;
//        }

//        private IShoppingListDTO GenerateDTOFromModel(ShoppingList dbList)
//        {
//            return new IShoppingListDTO()
//            {
//                shoppingListID = dbList.ShoppingListID,
//                shoppingListGroupID = dbList.Id,
//                shoppingListName = dbList.Name,
//                Items = new List<IItemDTO>(),
//                shoppingListGroupName = context.Groups.Find(dbList.Id).Name
//            };
//        }

//        public ShoppingList LoadToModel(IShoppingListDTO shoppingList)
//        {
//            if (GroupExists(shoppingList.shoppingListGroupID))
//            {
//                ShoppingList slInDb = GetSLFromDb(shoppingList);

//                if (slInDb != null)
//                    return slInDb;

//                else 
//                    return GenerateSLInDb(shoppingList);
//            }

//            return null;
//        }

//        private ShoppingList GenerateSLInDb(IShoppingListDTO shoppingList)
//        {
//            ShoppingList newList = new ShoppingList()
//            {
//                Name = shoppingList.shoppingListName,
//                GroupID = shoppingList.shoppingListGroupID,
//            };
//            context.ShoppingLists.Add(newList);
//            context.SaveChanges();
//            return newList;
//        }

//        private ShoppingList GetSLFromDb(IShoppingListDTO shoppingList)
//        {
//            ShoppingList list = context.ShoppingLists.Find(shoppingList.shoppingListID);
//            return list;
//        }

//        private bool GroupExists(int id)
//        {
//            var result = context.Groups.Find(id);
//            if (result == null)
//                return false;
//            else return true;
//        }

//    }
//}