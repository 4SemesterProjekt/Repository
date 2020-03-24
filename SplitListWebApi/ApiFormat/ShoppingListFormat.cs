﻿using System.Collections.Generic;

namespace ApiFormat
{
    public class ShoppingListFormat
    {
        public int shoppingListID { get; set; }
        public string shoppingListName { get; set; }
        public int shoppingListGroupID { get; set; }
        public string shoppingListGroupName { get; set; }
        public List<ItemFormat> Items { get; set; }

    }
}
