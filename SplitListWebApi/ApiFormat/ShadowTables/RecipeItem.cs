using ApiFormat.Item;
using ApiFormat.Recipe;

namespace ApiFormat.ShadowTables
{
    public class RecipeItem
    {
        public int ItemModelID { get; set; }
        public ItemModel ItemModel { get; set; }

        public int RecipeModelID { get; set; }
        public RecipeModel RecipeModel { get; set; }
    }
}