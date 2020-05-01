using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiFormat.ShadowTables;

namespace ApiFormat.Recipe
{
    public class RecipeModel : IModel
    {
        [Key]
        public int ModelId { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Method { get; set; }

        public List<RecipeItem> RecipeItems { get; set; }
    }
}