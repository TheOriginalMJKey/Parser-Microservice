using Application.Requests.Queries;

namespace Application.Requests.Interfaces
{
    public interface IGetSalesByDateQueryHandler
    {
        Task<List<Domain.Models.Sales>> Handle(GetSalesByDateQuery request);
    }
}