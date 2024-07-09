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
    /// Controller to know one client spendings.
    /// </summary>
    [ApiController]
    [Route("/ClientSpending")]
    [Authorize]
    public class SalesByClientsController : ControllerBase
    {
        private readonly IGetSalesByClientsQueryHandler _getSalesByClientsQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="SalesByClientsController"/> class.
        /// </summary>
        /// <param name="getSalesByClientsQueryHandler">The handler for getting sales by clients query.</param>
        public SalesByClientsController(IGetSalesByClientsQueryHandler getSalesByClientsQueryHandler)
        {
            _getSalesByClientsQueryHandler = getSalesByClientsQueryHandler;
        }

        /// <summary>
        /// Gets the sales by client.
        /// </summary>
        /// <param name="startDate">The start date of the period (format: YYYY-MM-DD).</param>
        /// <param name="endDate">The end date of the period (format: YYYY-MM-DD).</param>
        /// <param name="clientName">The name of the client.</param>
        /// <returns>A list of sales by clients.</returns>
        /// <response code="200">Returns the list of sales by clients.</response>
        /// <response code="400">If the provided start date, end date, or client name is invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<SalesByClientsViewModel>), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<List<SalesByClientsViewModel>>> GetSalesByClients([FromQuery] string startDate, [FromQuery] string endDate, [FromQuery] string clientName)
        {
            if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate) || string.IsNullOrEmpty(clientName))
            {
                return BadRequest("Please provide startDate, endDate, and clientName.");
            }

            if (!DateTime.TryParse(startDate, out DateTime parsedStartDate) ||
                !DateTime.TryParse(endDate, out DateTime parsedEndDate))
            {
                return BadRequest("Please provide valid dates in the format 'YYYY-MM-DD'.");
            }

            var query = new GetSalesByClientsQuery
            {
                StartDate = parsedStartDate,
                EndDate = parsedEndDate,
                ClientName = clientName
            };

            var result = await _getSalesByClientsQueryHandler.Handle(query);

            return Ok(result);
        }
    }
}
