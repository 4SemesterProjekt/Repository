using System.Collections.Generic;
using ApiFormat.ShadowTables;

namespace ApiFormat.Pantry
{
    public interface IPantryModel : IModel
    {
        public int GroupID { get; set; }
        public IGroupModel Group { get; set; }
        public ICollection<PantryItem> PantryItems { get; set; }
    }
}