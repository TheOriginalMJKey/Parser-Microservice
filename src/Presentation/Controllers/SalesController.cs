using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Application.SalesFolder.Handlers;
using Application.SalesFolder.Interfaces;
using Application.SalesFolder.Queries;
using Domain.Models;

namespace ProJect.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesController : ControllerBase
    {
        private readonly IGetSalesByDateQueryHandler _getSalesByDateQueryHandler;

        public SalesController(IGetSalesByDateQueryHandler getSalesByDateQueryHandler)
        {
            _getSalesByDateQueryHandler = getSalesByDateQueryHandler;
        }

        [HttpGet]
        public async Task<ActionResult<List<Sales>>> GetSales([FromQuery] string? startDate,
            [FromQuery] string? endDate, [FromQuery] string? customerName, [FromQuery] string? goodsName)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            {
                return BadRequest("Please provide both startDate and endDate in the format 'YYYY-MM-DD'.");
            }

            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate) ||
                !DateTime.TryParse(endDate, out DateTime parsedEndDate))
            {
                return BadRequest("Please provide valid dates in the format 'YYYY-MM-DD'.");
            }

            // Convert DateTime values to UTC
            DateTime utcStartDate = DateTime.SpecifyKind(parsedStartDate, DateTimeKind.Utc);
            DateTime utcEndDate = DateTime.SpecifyKind(parsedEndDate, DateTimeKind.Utc);

            var query = new GetSalesByDateQuery
            {
                StartDate = utcStartDate,
                EndDate = utcEndDate,
                CustomerName = customerName,
                GoodsName = goodsName
            };

            var result = await _getSalesByDateQueryHandler.Handle(query);

            return Ok(result);
        }
    }
}