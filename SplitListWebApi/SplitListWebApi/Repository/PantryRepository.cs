using ApiFormat;
using SplitListWebApi.Models;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace SplitListWebApi.Repository
{
    public interface IPantryRepository
    {
            void DeletePantry( PantryDTO pantry);
            PantryDTO UpdatePantry( PantryDTO pantry);
            PantryDTO GetPantryFromGroupID(int groupID);
    }

    public class PantryRepository : IPantryRepository
    {
        private SplitListContext context;
        private IItemRepository itemRepo;

        public PantryRepository(SplitListContext Context)
        {
            context = Context;
            itemRepo = new ItemRepository(context);
        }

        public void DeletePantry(PantryDTO pantry)
        {
            Pantry dbPantry = LoadToModel(pantry);
            if (dbPantry != null)
            {
                context.Pantries.Remove(dbPantry);
                if (pantry.Items != null)
                {
                    foreach (ItemDTO item in pantry.Items)
                    {
                        PantryItem dbItem = context.PantryItems.Find(pantry.ID, item.ItemID);
                        if (dbItem != null)
                        {
                            context.PantryItems.Remove(dbItem);
                        }
                    }
                }
            }
            context.SaveChanges();
        }

        public PantryDTO GetPantryFromGroupID(int groupID)
        {
            Pantry dbPantry = context.Pantries.Where(p => p.GroupID == groupID).SingleOrDefault();
            if (dbPantry != null)
            {
                PantryDTO newPantr = new PantryDTO()
                {
                    GroupID = dbPantry.GroupID,
                    ID = dbPantry.PantryID,
                    Name = dbPantry.Name,
                    Items = new List<ItemDTO>()
                };

                List<PantryItem> pantryItems = context.Pantries
                    .Where(p => p.PantryID == dbPantry.PantryID)
                    .Include(pi => pi.PantryItems)
                    .ThenInclude(it => it.Item)
                    .SelectMany(p => p.PantryItems)
                    .ToList();
                
                if (pantryItems != null)
                {
                    foreach (PantryItem item in pantryItems)
                    {
                        newPantr.Items.Add(new ItemDTO()
                        {
                            Amount = item.Amount,
                            ItemID = item.ItemID,
                            Name = item.Item.Name,
                            Type = item.Item.Type
                        });
                    }
                }
                newPantr.GroupName = context.Groups.Find(newPantr.GroupID).Name;
                return newPantr;
            }
            else return null;
        }

        private void RemoveItemsFromPantry(PantryDTO pantry)
        {
            List<PantryItem> dbItemsInPantry = context.PantryItems
                .Where(pi => pi.PantryID == pantry.ID)
                .Include(it => it.Item)
                .ToList();

            foreach (PantryItem pantryItem in dbItemsInPantry)
            {
                ItemDTO itemToRemove = pantry.Items.Find(it => it.ItemID == pantryItem.ItemID);
                if (itemToRemove == null)
                {
                    context.PantryItems.Remove(pantryItem);
                }
            }
            context.SaveChanges();
        }

        private void AddItemsToPantry(PantryDTO pantry)
        {
            foreach (ItemDTO item in pantry.Items)
            {
                Item itemModel = itemRepo.LoadToModel(item);
                if (item.ItemID <= 0)
                    item.ItemID = itemModel.ItemID;

                PantryItem pantryItemModel = context.PantryItems.Find(pantry.ID, item.ItemID);
                if (pantryItemModel != null)
                {
                    pantryItemModel.Amount = item.Amount;
                    context.Update(pantryItemModel);
                    context.SaveChanges();
                }
                else
                {
                    context.PantryItems.Add(new PantryItem()
                    {
                        ItemID = item.ItemID,
                        Amount = item.Amount,
                        PantryID = pantry.ID
                    });
                    context.SaveChanges();
                }
            }
        }

        public PantryDTO UpdatePantry(PantryDTO pantry)
        {
            Pantry dbPantry = LoadToModel(pantry);

            if (dbPantry != null)
            {
                if (pantry.ID <= 0)
                {
                    pantry.ID = dbPantry.PantryID;
                }

                dbPantry.Name = pantry.Name;
                context.Pantries.Update(dbPantry);
                context.SaveChanges();

                if (pantry.Items == null)
                {
                    pantry.Items = new List<ItemDTO>();
                }
                else
                {
                    RemoveItemsFromPantry(pantry);
                    AddItemsToPantry(pantry);
                }
            }
            return pantry;
        }

        private Pantry LoadToModel(PantryDTO pantry)
        {
            if (context.Groups.Find(pantry.GroupID) != null)
            {
                Pantry dbPantry = context.Pantries.Find(pantry.ID);

                if (dbPantry != null)
                    return dbPantry;
                else
                {
                    Pantry newPantry = new Pantry()
                    {
                        Name = pantry.Name,
                        GroupID = pantry.GroupID,
                    };

                    context.Pantries.Add(newPantry);
                    context.SaveChanges();
                    return newPantry;
                }
            }
            return null;
        }
    }
}
