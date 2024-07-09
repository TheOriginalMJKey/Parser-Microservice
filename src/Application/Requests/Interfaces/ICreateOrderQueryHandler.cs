using Application.Requests.Queries;

namespace Application.Requests.Interfaces
{
    public interface ICreateOrderQueryHandler
    {
        Task Handle(CreateOrderQuery request);
    }
}