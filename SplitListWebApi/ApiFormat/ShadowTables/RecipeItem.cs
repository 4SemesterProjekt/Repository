using ApiFormat.Item;
using ApiFormat.Recipe;

namespace ApiFormat.ShadowTables
{
    public class RecipeItem
    {
        public double ItemModelID { get; set; }
        public ItemModel ItemModel { get; set; }

        public double RecipeModelID { get; set; }
        public RecipeModel RecipeModel { get; set; }
    }
}