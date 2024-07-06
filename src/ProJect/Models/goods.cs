namespace ProJect.Models;

public class goods
{
    public int GoodsId { get; set; }
    public string GoodsName { get; set; }
    public List<prices> prices { get; set; }
    public List<sales> sales { get; set; }
}