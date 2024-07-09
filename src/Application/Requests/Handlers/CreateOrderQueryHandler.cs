using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Infrastructure.Repositories;

namespace Application.Requests.Handlers
{
    public class CreateOrderQueryHandler : ICreateOrderQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public CreateOrderQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(CreateOrderQuery request)
        {
            await _saleRepository.CreateOrder(request.Order);
        }
    }
}