using System.Collections.Generic;
using ApiFormat.Item;

namespace ApiFormat.Recipe
{
    public interface IRecipeDTO : IDTO
    {
        List<IItemDTO> Items { get; set; }
    }
}