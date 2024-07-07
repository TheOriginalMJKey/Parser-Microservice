using Application.SalesFolder.Queries;
using Domain.Models;

namespace Application.SalesFolder.Interfaces
{
    public interface IGetSalesByDateQueryHandler
    {
        Task<List<Sales>> Handle(GetSalesByDateQuery request);
    }
}