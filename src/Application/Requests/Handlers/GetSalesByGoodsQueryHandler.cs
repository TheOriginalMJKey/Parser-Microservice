using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Domain.ViewModels;
using Infrastructure.Repositories;

namespace Application.Requests.Handlers
{
    public class GetSalesByGoodsQueryHandler : IGetSalesByGoodsQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByGoodsQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<List<SalesByGoodsViewModel>> Handle(GetSalesByGoodsQuery request)
        {
            return await _saleRepository.GetSalesByGoods(request.StartDate, request.EndDate, request.GoodsName);
        }
    }
}