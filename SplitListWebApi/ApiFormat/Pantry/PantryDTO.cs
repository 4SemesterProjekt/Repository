using System.Collections.Generic;
using ApiFormat.Group;
using ApiFormat.Item;

namespace ApiFormat.Pantry
{
    public class PantryDTO : IDTO
    {
        public int GroupID { get; set; }
        public GroupDTO Group { get; set; }
        public List<ItemDTO> Items { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
    }
}