using Application.SalesFolder.Queries;
using Domain.Models;

namespace Application.SalesFolder.Interfaces
{
    public interface IGetSalesByDateQueryHandler
    {
        Task<List<Domain.Models.Sales>> Handle(GetSalesByDateQuery request);
    }
}