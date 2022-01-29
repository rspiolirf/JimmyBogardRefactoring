using JimmyRefactoring.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace JimmyRefactoring.Features.Members.FindAllMembers;

[ApiController]
public class FindAllMembersController : ControllerBase
{
    [HttpGet("members")]
    public async Task<IActionResult> FindAllMembers([FromServices] AppDbContext context)
    {
        var members = await context.Members
            .Include(o => o.AssignedOffers)
            .ToListAsync();
            
        return Ok(members);
    }
}