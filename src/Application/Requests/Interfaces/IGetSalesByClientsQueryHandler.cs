using Application.Requests.Queries;
using Domain.ViewModels;

namespace Application.Requests.Interfaces
{
    public interface IGetSalesByClientsQueryHandler
    {
        Task<List<SalesByClientsViewModel>> Handle(GetSalesByClientsQuery request);
    }
}