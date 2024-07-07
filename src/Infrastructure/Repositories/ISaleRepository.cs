using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Models;

namespace Application.Interfaces
{
    public interface ISaleRepository
    {
        Task<List<Sales>> GetSalesByDate(DateTime startDate, DateTime endDate, string? customerName, string? goodsName);
    }
}