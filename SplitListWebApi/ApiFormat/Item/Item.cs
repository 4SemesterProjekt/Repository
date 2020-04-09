using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Item
{
    public class Item : ItemModel, IItemDTO
    {
        public int Amount { get; set; }
    }
}