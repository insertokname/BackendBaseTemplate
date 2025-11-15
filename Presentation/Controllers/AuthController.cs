using System.Security.Claims;

using Application.Commands.Users;
using Application.Commands.Users.CreateUser;
using Application.Commands.Users.LoginUser;

using Domain.Entities;

using Infrastructure.Token;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(
        CreatePasswordUserHandler createUserHandler,
        CreateGoogleUserHandler createGoogleUserHandler,
        LoginPasswordUserHandler loginPasswordUserHandler,
        TokenGeneratorService tokenProvider) : ControllerBase
    {
        [HttpPost("CreateUser")]
        public async Task<ActionResult<string>> CreateUser([FromBody] CreatePasswordUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(
                await createUserHandler.HandleAsync(command)
            );
        }

        [HttpPost("Login")]
        [EnableRateLimiting("LoginRateLimiter")]
        public async Task<ActionResult<string>> Login([FromBody] LoginPasswordUserCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            return Ok(
                await loginPasswordUserHandler.HandleAsync(command)
            );
        }

        [HttpGet("Google")]
        public ActionResult AuthGoogle()
        {
            var properties = new AuthenticationProperties
            {
                RedirectUri = Url.Action(nameof(AuthGoogleReturn))
            };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        [HttpGet("GoogleReturn")]
        public async Task<ActionResult<string>> AuthGoogleReturn()
        {
            var authenticateResult = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

            if (!authenticateResult.Succeeded || authenticateResult.Principal == null)
            {
                return BadRequest(new { Error = "Google authentication failed." });
            }

            var principal = authenticateResult.Principal;
            var googleId = principal.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = principal.FindFirstValue(ClaimTypes.Email);

            if (string.IsNullOrEmpty(googleId) || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Error = "Could not retrieve Google ID or email." });
            }

            User user = await createGoogleUserHandler.HandleAsync(
                new CreateGoogleUserCommand { Email = email, GoogleId = googleId }
            );

            var token = tokenProvider.Create(user);

            return token;
        }

        [HttpGet("GoogleAccessDenied")]
        public ActionResult AuthGoogleAccessDenied()
        {
            return Unauthorized(new { Error = "Access denied by Google or the user.", Description = "The user may have cancelled the login or an error occurred on Google's side." });
        }
    }
}