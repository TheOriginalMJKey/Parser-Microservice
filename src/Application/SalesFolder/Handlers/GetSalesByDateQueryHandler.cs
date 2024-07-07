﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.SalesFolder.Interfaces;
using Application.SalesFolder.Queries;
using Domain.Models;


namespace Application.SalesFolder.Handlers
{
    public class GetSalesByDateQueryHandler : IGetSalesByDateQueryHandler
    {
        private readonly ISaleRepository _saleRepository;

        public GetSalesByDateQueryHandler(ISaleRepository saleRepository)
        {
            _saleRepository = saleRepository;
        }

        public async Task<List<Sales>> Handle(GetSalesByDateQuery request)
        {
            return await _saleRepository.GetSalesByDate(request.StartDate, request.EndDate, request.CustomerName, request.GoodsName);
        }
    }
}