using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Application.Sales.Queries;
using Application.Interfaces;
using Domain.Models;
using Microsoft.OpenApi.Models;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SalesByMonthController : ControllerBase
    {
        private readonly IGetSalesByMonthQueryHandler _getSalesByMonthQueryHandler;

        public SalesByMonthController(IGetSalesByMonthQueryHandler getSalesByMonthQueryHandler)
        {
            _getSalesByMonthQueryHandler = getSalesByMonthQueryHandler;
        }

        /// <summary>
        /// Gets the sales by month.
        /// </summary>
        /// <remarks>
        /// Returns a list of sales for the specified year and month.
        /// </remarks>
        /// <response code="200">Returns the list of sales</response>
        /// <response code="400">If the year or month are invalid</response>
        /// <param name="year">The year to get sales for.</param>
        /// <param name="month">The month to get sales for.</param>
        [HttpGet]
        [ProducesResponseType(typeof(List<Sales>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<ActionResult<List<Sales>>> GetSalesByMonth([FromQuery] int year, [FromQuery] int month)
        {
            if (year <= 0 || month < 1 || month > 12)
            {
                return BadRequest("Please provide valid year and month values.");
            }

            var query = new GetSalesByMonthQuery
            {
                Year = year,
                Month = month
            };

            var result = await _getSalesByMonthQueryHandler.Handle(query);

            return Ok(result);
        }
    }
}