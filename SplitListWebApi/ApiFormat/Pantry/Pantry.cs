using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.Pantry
{
    public class Pantry : PantryModel, IPantryDTO
    {
        public string GroupName { get; set; }
        public List<IItemDTO> Items { get; set; }
    }
}