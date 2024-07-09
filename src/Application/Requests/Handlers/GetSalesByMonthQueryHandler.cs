using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Sales.Queries;
using Application.Interfaces;
using Domain.ViewModels;
using Infrastructure.Repositories;

namespace Application.Sales.Handlers
{
    public class GetSalesByMonthQueryHandler : IGetSalesByMonthQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByMonthQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<List<Domain.Models.Sales>> Handle(GetSalesByMonthQuery request)
        {
            return await _saleRepository.GetSalesByMonth(request.Year, request.Month);
        }
    }
}