using System.Threading.Tasks;
using Application.Requests.Queries;

namespace Application.Interfaces
{
    public interface IDeleteOrderQueryHandler
    {
        Task Handle(DeleteOrderQuery request);
    }
}