using System;
using System.Collections.Generic;
using System.Text;

namespace ApiFormat
{
    class ItemFormat
    {
        public int ItemID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } 
        public List<PantryFormat> Pantries { get; set; }
        public List<ShoppingListFormat> ShoppingLists { get; set; }
        public List<RecipeFormat> Recipes { get; set; }
    }
}
