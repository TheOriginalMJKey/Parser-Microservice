using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Infrastructure.Repositories;

namespace Application.Requests.Handlers
{
    public class GetSalesByDateQueryHandler : IGetSalesByDateQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByDateQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<List<Domain.Models.Sales>> Handle(GetSalesByDateQuery request)
        {
            return await _saleRepository.GetSalesByDate(request.StartDate, request.EndDate, request.CustomerName, request.GoodsName);
        }
    }
}