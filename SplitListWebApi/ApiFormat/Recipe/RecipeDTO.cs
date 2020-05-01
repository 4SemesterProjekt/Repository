using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Recipe
{
    public class RecipeDTO : IDTO
    {
        public List<ItemDTO> Items { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Method { get; set; }
    }
}