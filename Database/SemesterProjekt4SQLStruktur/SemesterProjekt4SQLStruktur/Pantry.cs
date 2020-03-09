using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SemesterProjekt4SQLStruktur
{
    public class Pantry
    {
        public int PantryID { get; set; }
        public string Name { get; set; }
        
        [ForeignKey("GroupID")]
        public Group Group { get; set; }
        
        
        public ICollection<PantryItem> PantryItems { get; set; }
    }

    public class PantryItem
    {
        public int PantryID { get; set; }
        public Pantry Pantry { get; set; }
        
        public int ItemID { get; set; }
        public Item Item { get; set; }
        
        public int Amount { get; set; }
    }
}