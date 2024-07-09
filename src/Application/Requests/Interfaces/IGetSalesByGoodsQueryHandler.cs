using Application.Requests.Queries;
using Domain.ViewModels;

namespace Application.Requests.Interfaces
{
    public interface IGetSalesByGoodsQueryHandler
    {
        Task<List<SalesByGoodsViewModel>> Handle(GetSalesByGoodsQuery request);
    }
}