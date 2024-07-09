using Domain.Models;

namespace Application.Requests.Queries
{
    public class CreateOrderQuery
    {
        public PostOrderModel Order { get; set; }
    }
}