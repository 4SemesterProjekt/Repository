using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFormat.Group;
using ApiFormat.ShadowTables;

namespace ApiFormat.Pantry
{
    public class PantryModel : IModel
    {
        public double Id { get; set; }
        public string Name { get; set; }
        public double GroupModelID { get; set; }
        public GroupModel GroupModel { get; set; }
        public List<PantryItem> PantryItems { get; set; }
    }
}