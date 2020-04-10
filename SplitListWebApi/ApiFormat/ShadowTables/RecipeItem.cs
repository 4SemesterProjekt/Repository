using ApiFormat.Item;
using ApiFormat.Recipe;

namespace ApiFormat.ShadowTables
{
    public class RecipeItem
    {
        public double ItemID { get; set; }
        public ItemModel item { get; set; }

        public double RecipeID { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}