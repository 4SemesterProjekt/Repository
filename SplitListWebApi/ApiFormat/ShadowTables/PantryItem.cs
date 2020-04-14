using ApiFormat.Item;
using ApiFormat.Pantry;

namespace ApiFormat.ShadowTables
{
    public class PantryItem
    {
        public double PantryModelID { get; set; }
        public PantryModel PantryModel { get; set; }
        public double ItemModelID { get; set; }
        public ItemModel ItemModel { get; set; }
        public int Amount { get; set; }
    }
}