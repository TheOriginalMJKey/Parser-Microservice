using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests.Queries;
using Domain.Models;
using Infrastructure.Repositories;

namespace Application.Sales.Handlers
{
    public class DeleteOrderQueryHandler : IDeleteOrderQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public DeleteOrderQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task Handle(DeleteOrderQuery request)
        {
            await _saleRepository.DeleteOrder(request.Order);
        }
    }
}