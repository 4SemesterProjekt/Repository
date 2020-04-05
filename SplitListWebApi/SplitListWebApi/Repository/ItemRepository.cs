using ApiFormat;
using SplitListWebApi.Areas.Identity.Data;
using SplitListWebApi.Areas.Identity.Data.Models;
using SplitListWebApi.Models;

namespace SplitListWebApi.Repository
{
    public interface IItemRepository
    {
        Item LoadToModel(ItemDTO item);
    }

    public class ItemRepository : IItemRepository
    {
        private SplitListContext context;

        public ItemRepository(SplitListContext Context)
        {
            context = Context;
        }

        public Item LoadToModel(ItemDTO item)
        {
            Item dbItem = context.Items.Find(item.ItemID);

            if (dbItem != null)
                return dbItem;
            else
            {
                Item newItem = new Item()
                {
                    Name = item.Name,
                    Type = item.Type
                };

                context.Items.Add(newItem);
                context.SaveChanges();
                return newItem;
            }
        }
    }
}