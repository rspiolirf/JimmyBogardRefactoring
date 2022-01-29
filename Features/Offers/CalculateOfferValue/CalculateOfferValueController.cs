using Microsoft.AspNetCore.Mvc;

namespace JimmyRefactoring.Features.Offers.CalculateOfferValue;

[ApiController]
public class CalculateOfferValueController : ControllerBase
{
    [HttpGet("calculate-offer-value")]
    public IActionResult CalculateOfferValue()
    {
        return Ok(79);
    }
}