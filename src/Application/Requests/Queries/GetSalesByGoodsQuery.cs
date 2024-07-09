namespace Application.Requests.Queries
{
    public class GetSalesByGoodsQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string GoodsName { get; set; }
    }
}