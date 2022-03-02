using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace JimmyRefactoring.Features.Offers.AssignOffer;

[ApiController]
public class AssignOfferController : ControllerBase
{
    [HttpGet("assign-offer")]
    public async Task<IActionResult> AssignOffer([FromServices] IMediator mediator,
                                                 [FromQuery] AssignOfferRequest request)
    {
        await mediator.Send(request);
        return Ok(new { MemberId = request.MemberId, OfferTypeId = request.OfferTypeId });
    }
}