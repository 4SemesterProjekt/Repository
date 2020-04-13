using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Recipe
{
    public class RecipeDTO : IDTO
    {
        public List<ItemDTO> Items { get; set; }
        public double Id { get; set; }
        public string Name { get; set; }
    }
}