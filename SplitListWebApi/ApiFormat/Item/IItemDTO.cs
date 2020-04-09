namespace ApiFormat.Item
{
    public interface IItemDTO : IDTO
    {
        string Type { get; set; }
        int Amount { get; set; }
    }
}