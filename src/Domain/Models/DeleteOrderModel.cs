namespace Domain.Models
{
    public class DeleteOrderModel
    {
        public DateTime SalesDate { get; set; }
        public int CustomersId { get; set; }
        public int GoodsId { get; set; }
    }
}