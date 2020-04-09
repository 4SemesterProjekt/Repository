using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Recipe
{
    public interface IRecipeDTO : IDTO
    {
        public ICollection<IItemDTO> Items { get; set; }
    }
}