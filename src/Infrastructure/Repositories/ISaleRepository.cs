using Domain.Models;
using Domain.ViewModels;

namespace Infrastructure.Repositories
{
    public interface ISaleRepository
    {
        Task<List<Sales>> GetSalesByDate(DateTime startDate, DateTime endDate, string? customerName, string? goodsName);
        Task<List<SalesByGoodsViewModel>> GetSalesByGoods(DateTime startDate, DateTime endDate, string goodsName);
        Task<List<SalesByClientsViewModel>> GetSalesByClients(DateTime startDate, DateTime endDate, string clientName);
        Task CreateOrder(PostOrderModel order);
        Task DeleteOrder(DeleteOrderModel order);
        Task<List<Sales>> GetSalesByMonth(int year, int month);
    }
}