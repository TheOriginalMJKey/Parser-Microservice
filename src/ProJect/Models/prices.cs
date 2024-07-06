namespace ProJect.Models;

public class prices
{
    public int GoodsId { get; set; }
    public decimal PriceValue { get; set; }
    public goods goods { get; set; }
}