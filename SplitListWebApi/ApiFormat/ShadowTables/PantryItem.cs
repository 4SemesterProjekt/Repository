using ApiFormat.Item;
using ApiFormat.Pantry;

namespace ApiFormat.ShadowTables
{
    public class PantryItem
    {
        public double PantryID { get; set; }
        public PantryModel Pantry { get; set; }
        public double ItemID { get; set; }
        public ItemModel Item { get; set; }
        public int Amount { get; set; }
    }
}