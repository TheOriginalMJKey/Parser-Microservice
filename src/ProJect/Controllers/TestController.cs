using Microsoft.AspNetCore.Mvc;

namespace ProJect.Controllers;
using Microsoft.AspNetCore.Mvc;
using System;

using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc;
using System;

[ApiController]
[Route("[controller]")]
public class DateController : ControllerBase
{
    /// <summary>
    /// Get date range.
    /// </summary>
    /// <param name="start">The start date.</param>
    /// <param name="end">The end date.</param>
    /// <returns>The number of days between the two dates.</returns>
    /// <response code="200">The operation was successful.</response>
    /// <response code="400">The operation failed due to invalid input.</response>
    [HttpGet]
    public IActionResult GetDateRange([FromQuery] DateTime? start, [FromQuery] DateTime? end)
    {
        if (!start.HasValue || !end.HasValue)
        {
            return BadRequest("Both start and end dates must be provided.");
        }

        TimeSpan diff = end.Value - start.Value;
        int days = diff.Days;

        return Ok(days);
    }
}



  
public class DateRange
{
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
}
