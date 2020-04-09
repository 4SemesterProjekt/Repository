using System.Collections.Generic;
using ApiFormat.Item;
using ApiFormat.ShadowTables;

namespace ApiFormat.Pantry
{
    public class Pantry : IPantryModel, IPantryDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int GroupID { get; set; }
        public string GroupName { get; set; }
        public ICollection<IItemDTO> Items { get; set; }
        public IGroupModel Group { get; set; }
        public ICollection<PantryItem> PantryItems { get; set; }
    }
}