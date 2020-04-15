namespace ApiFormat.Item
{
    public class ItemDTO : IDTO
    {
        public string Type { get; set; }
        public int Amount { get; set; }
        public int ModelId { get; set; }
        public string Name { get; set; }
    }
}