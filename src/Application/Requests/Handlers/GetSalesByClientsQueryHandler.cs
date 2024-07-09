using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Domain.ViewModels;
using Infrastructure.Repositories;

namespace Application.Requests.Handlers
{
    public class GetSalesByClientsQueryHandler : IGetSalesByClientsQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByClientsQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<List<SalesByClientsViewModel>> Handle(GetSalesByClientsQuery request)
        {
            return await _saleRepository.GetSalesByClients(request.StartDate, request.EndDate, request.ClientName);
        }
    }
}