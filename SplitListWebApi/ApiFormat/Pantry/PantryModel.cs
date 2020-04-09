using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Pantry
{
    public class PantryModel : IModel
    {
        public int GroupID { get; set; }
        public GroupModel Group { get; set; }
        public List<PantryItem> PantryItems { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
    }
}