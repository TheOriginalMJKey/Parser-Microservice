using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Sales.Queries;

namespace Application.Interfaces
{
    public interface IGetSalesByMonthQueryHandler
    {
        Task<List<Domain.Models.Sales>> Handle(GetSalesByMonthQuery request);
    }
}