using Application.Requests.Interfaces;

namespace Application.Requests.Queries
{
    public class GetSalesByDateQuery : IGetSalesByDateQuery
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? CustomerName { get; set; }
        public string? GoodsName { get; set; }
    }
}