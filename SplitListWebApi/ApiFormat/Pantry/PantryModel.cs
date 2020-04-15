using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ApiFormat.Group;
using ApiFormat.ShadowTables;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace ApiFormat.Pantry
{
    public class PantryModel : IModel
    {
        [Key]
        public int ModelId { get; set; }
        public string Name { get; set; }

        public int GroupModelModelID { get; set; }
        public GroupModel GroupModel { get; set; }
        public List<PantryItem> PantryItems { get; set; }
    }
}