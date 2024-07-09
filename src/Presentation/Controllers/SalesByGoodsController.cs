using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProJect.Controllers
{
    /// <summary>
    /// Controller to return amount of sold good
    /// </summary>
    [ApiController]
    [Route("/Sales/GoodSold")]
    [Authorize]
    public class SalesByGoodsController : ControllerBase
    {
        private readonly IGetSalesByGoodsQueryHandler _getSalesByGoodsQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesByGoodsController"/> class.
        /// </summary>
        /// <param name="getSalesByGoodsQueryHandler">The handler for getting sales by goods query.</param>
        public SalesByGoodsController(IGetSalesByGoodsQueryHandler getSalesByGoodsQueryHandler)
        {
            _getSalesByGoodsQueryHandler = getSalesByGoodsQueryHandler;
        }

        /// <summary>
        /// Gets the sales by goods.
        /// </summary>
        /// <param name="startDate">The start date of the period (format: YYYY-MM-DD).</param>
        /// <param name="endDate">The end date of the period (format: YYYY-MM-DD).</param>
        /// <param name="goodsName">The name of the goods.</param>
        /// <returns>A list of sales by goods.</returns>
        /// <response code="200">Returns the list of sales by goods.</response>
        /// <response code="400">If the provided start date, end date, or goods name is invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<SalesByGoodsViewModel>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<SalesByGoodsViewModel>>> GetSalesByGoods([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string goodsName)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(goodsName))
            {
                return BadRequest("Please provide startDate, endDate, and goodsName.");
            }

            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate) ||
                !DateTime.TryParse(endDate, out DateTime parsedEndDate))
            {
                return BadRequest("Please provide valid dates in the format 'YYYY-MM-DD'.");
            }

            var query = new GetSalesByGoodsQuery
            {
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                GoodsName = goodsName
            };

            var result = await _getSalesByGoodsQueryHandler.Handle(query);

            return Ok(result);
        }
    }
}
