using ApiFormat.Item;
using ApiFormat.Recipe;

namespace ApiFormat.ShadowTables
{
    public class RecipeItem
    {
        public int ItemID { get; set; }
        public IItemModel item { get; set; }

        public int RecipeID { get; set; }
        public IRecipeModel Recipe { get; set; }
    }
}