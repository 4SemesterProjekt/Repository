﻿using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.ShoppingList
{
    public class ShoppingList : ShoppingListModel, IShoppingListDTO
    {
        public string GroupName { get; set; }
        public List<IItemDTO> Items { get; set; }
    }
}