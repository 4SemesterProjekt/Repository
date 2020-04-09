using ApiFormat.Item;
using ApiFormat.Recipe;

namespace ApiFormat.ShadowTables
{
    public class RecipeItem
    {
        public int ItemID { get; set; }
        public ItemModel item { get; set; }

        public int RecipeID { get; set; }
        public RecipeModel Recipe { get; set; }
    }
}