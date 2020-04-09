//using ApiFormat;
//using ApiFormat.Item;
//using SplitListWebApi.Areas.Identity.Data;

//namespace SplitListWebApi.Repository
//{
//    public interface IItemRepository
//    {
//        Item LoadToModel(IItemDTO item);
//    }

//    public class ItemRepository : IItemRepository
//    {
//        private SplitListContext context;

//        public ItemRepository(SplitListContext Context)
//        {
//            context = Context;
//        }

//        public Item LoadToModel(IItemDTO item)
//        {
//            Item dbItem = context.Items.Find(item.Id);

//            if (dbItem != null)
//            {
//                dbItem.Name = item.Name;
//                dbItem.Type = item.Type;
//                return dbItem;
//            }
                
//            else
//            {
//                Item newItem = new Item()
//                {
//                    Name = item.Name,
//                    Type = item.Type
//                };

//                context.Items.Add(newItem);
//                context.SaveChanges();
//                return newItem;
//            }
//        }
//    }
//}