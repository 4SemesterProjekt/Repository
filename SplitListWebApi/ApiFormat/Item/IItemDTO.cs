namespace ApiFormat.Item
{
    public interface IItemDTO : IDTO
    {
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}