using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;
using Domain.ViewModels;

namespace Application.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sales>> GetSalesByDate(DateTime startDate, DateTime endDate, string? customerName, string? goodsName);
        Task<List<SalesByGoodsViewModel>> GetSalesByGoods(DateTime startDate, DateTime endDate, string goodsName);
    }
}