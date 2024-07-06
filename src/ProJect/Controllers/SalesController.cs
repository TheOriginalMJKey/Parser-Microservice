using ProJect.Models;

namespace ProJect.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

[ApiController]
[Route("[controller]")]
public class SalesByDate : ControllerBase
{
    private readonly MyDbContext _context;

    public SalesByDate(MyDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get date range.
    /// </summary>
    /// <param name="start">The start date.</param>
    /// <param name="end">The end date.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <response code="200">The operation was successful.</response>
    /// <response code="400">The operation failed due to invalid input.</response>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<sales>>> Sales([FromQuery] string? startDate,
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

        // Build the SQL query
        string sqlQuery = "SELECT DISTINCT sales.* " +
                          "FROM sales " +
                          "JOIN goods ON sales.goods_id = goods.goods_id " +
                          "JOIN customers ON sales.customers_id = customers.customers_id " +
                          "WHERE sales.sales_date >= {0} " +
                          "  AND sales.sales_date <= {1}";

        // Add conditions for customerName and goodsName if they are not null or empty
        if (!string.IsNullOrEmpty(customerName))
        {
            sqlQuery += " AND customers.customers_name = {2}";
        }

        if (!string.IsNullOrEmpty(goodsName))
        {
            sqlQuery += " AND goods.goods_name = {3}";
        }

        // Execute the SQL query
        var result = await _context.Sales
            .FromSqlRaw(
                sqlQuery,
                utcStartDate, utcEndDate,
                string.IsNullOrEmpty(customerName) ? null : customerName,
                string.IsNullOrEmpty(goodsName) ? null : goodsName)
            .ToListAsync();

        return Ok(result);
    }
}








