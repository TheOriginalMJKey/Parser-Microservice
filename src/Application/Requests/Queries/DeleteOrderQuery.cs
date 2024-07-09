using Domain.Models;

namespace Application.Requests.Queries
{
    public class DeleteOrderQuery
    {
        public DeleteOrderModel Order { get; set; }
    }
}