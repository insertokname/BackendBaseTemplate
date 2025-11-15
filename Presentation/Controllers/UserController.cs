using System.Security.Claims;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Application.Commands.Users;
using Application.Query.GenericQueries;
using Application.Query.Users;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(
        GetByIdHandler<User> getByIdHandler,
        GetSelfHandler getSelfHandler,
        SetAdminHandler setAdminHandler) : ControllerBase
    {

        [Authorize]
        [HttpGet("Self")]
        public async Task<ActionResult<GetSelfResponse>> GetSelf()
        {
            if (!Guid.TryParse(User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var userId))
            {
                return Forbid();
            }

            return Ok(
                await getSelfHandler.HandleAsync(new GetSelfQuery() { SelfId = userId })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("ById")]
        public async Task<ActionResult<User>> GetById([FromQuery] Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(
                await getByIdHandler.HandleAsync(new GetByIdQuery() { EntityId = id })
            );
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("SetAdmin")]
        public async Task<ActionResult> SetAdmin([FromBody] SetAdminCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            await setAdminHandler.HandleAsync(command);
            return Ok();
        }
    }
}
