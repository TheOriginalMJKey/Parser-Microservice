using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;
using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Hosting;

namespace ProJect.Controllers
{
    /// <summary>
    /// Controller to manage sales.
    /// </summary>
    [ApiController]
    [Route("/Sales/SalesbyAll")]
    [Authorize]
    public class SalesController : ControllerBase
    {
        private readonly IGetSalesByDateQueryHandler _getSalesByDateQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesController"/> class.
        /// </summary>
        /// <param name="getSalesByDateQueryHandler">The handler for getting sales by date query.</param>
        public SalesController(IGetSalesByDateQueryHandler getSalesByDateQueryHandler)
        {
            _getSalesByDateQueryHandler = getSalesByDateQueryHandler;
        }

        /// <summary>
        /// Gets the sales by date, customer name, and goods name.
        /// </summary>
        /// <param name="startDate">The start date of the period (format: YYYY-MM-DD).</param>
        /// <param name="endDate">The end date of the period (format: YYYY-MM-DD).</param>
        /// <param name="customerName">The name of the customer (optional).</param>
        /// <param name="goodsName">The name of the goods (optional).</param>
        /// <returns>A list of sales.</returns>
        /// <response code="200">Returns the list of sales.</response>
        /// <response code="400">If the provided start date or end date is invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<Sales>), 200)]
        [ProducesResponseType(400)]
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
