using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Application.Interfaces;
using Application.Requests.Interfaces;
using Application.Requests.Queries;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;

namespace Presentation.Controllers
{
    /// <summary>
    /// Controller to manage orders.
    /// </summary>
    [ApiController]
    [Route("OrderManage/")]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly ICreateOrderQueryHandler _createOrderQueryHandler;
        private readonly IDeleteOrderQueryHandler _deleteOrderQueryHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="OrdersController"/> class.
        /// </summary>
        /// <param name="createOrderQueryHandler">The handler for creating an order.</param>
        /// <param name="deleteOrderQueryHandler">The handler for deleting an order.</param>
        public OrdersController(ICreateOrderQueryHandler createOrderQueryHandler,
            IDeleteOrderQueryHandler deleteOrderQueryHandler)
        {
            _createOrderQueryHandler = createOrderQueryHandler;
            _deleteOrderQueryHandler = deleteOrderQueryHandler;
        }

        /// <summary>
        /// Creates a new order.
        /// </summary>
        /// <param name="order">The order details.</param>
        /// <returns>A response indicating whether the order was created successfully.</returns>
        /// <response code="200">The order was created successfully.</response>
        /// <response code="400">The order details are invalid.</response>
        [HttpPost("create")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> PostOrder([FromBody] PostOrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = new CreateOrderQuery
            {
                Order = order
            };

            await _createOrderQueryHandler.Handle(query);

            return Ok();
        }

        /// <summary>
        /// Deletes an existing order.
        /// </summary>
        /// <param name="order">The order details to delete.</param>
        /// <returns>A response indicating whether the order was deleted successfully.</returns>
        /// <response code="200">The order was deleted successfully.</response>
        /// <response code="400">The order details are invalid.</response>
        [HttpDelete("delete")]
        [ProducesResponseType(typeof(void), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteOrder([FromBody] DeleteOrderModel order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var query = new DeleteOrderQuery
            {
                Order = order
            };

            await _deleteOrderQueryHandler.Handle(query);

            return Ok();
        }
    }
}
