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
            void UpdatePantry( PantryDTO pantry);
            PantryDTO GetPantryFromGroupID(int groupID);
    }

    public class PantryRepository : IPantryRepository
    {
        private SplitListContext context;

        public PantryRepository(SplitListContext Context)
        {
            context = Context;
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
                        PantryItem dbItem = context.PantryItems.Find(item.ItemID);
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

        public void UpdatePantry(PantryDTO pantry)
        {
            Pantry dbPantry = LoadToModel(pantry);

            if (dbPantry != null)
            {
                if (pantry.Name != dbPantry.Name)
                {
                    dbPantry.Name = pantry.Name;
                    context.Pantries.Update(dbPantry);
                    context.SaveChanges();
                }

                if (pantry.Items == null)
                {
                    pantry.Items = new List<ItemDTO>();
                }
                else
                {
                    foreach (ItemDTO item in pantry.Items)
                    {
                        Item dbItem = context.Items.Find(item.ItemID);
                        if (dbItem != null)
                        {
                            dbItem.Name = item.Name;
                            dbItem.Type = item.Type;
                            context.Items.Update(dbItem);
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

                        PantryItem dbPantryItem = context.PantryItems.Find(pantry.ID, item.ItemID);
                        if (dbPantryItem != null)
                        {
                            dbPantryItem.Amount = item.Amount;
                            context.PantryItems.Update(dbPantryItem);
                            context.SaveChanges();
                        }
                        else
                        {
                            context.PantryItems.Add(new PantryItem() 
                            { 
                                ItemID = item.ItemID,
                                PantryID = pantry.ID,
                                Amount = item.Amount
                            });
                            context.SaveChanges();
                        }
                    }
                }
            }
        }

        public PantryDTO AddPantry(PantryDTO newPantry)
        {
            PantryDTO newPantryDTO = newPantry;
            Pantry pantryModel = LoadToModel(newPantry);
            newPantryDTO.ID = pantryModel.PantryID;
            return newPantryDTO;
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
