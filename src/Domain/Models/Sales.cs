namespace Domain.Models
{
    public class Sales
    {
        public string? SalesDate { get; set; }
        public int CustomersId { get; set; }
        public int GoodsId { get; set; }
        public int Quantity { get; set; }
    }
}