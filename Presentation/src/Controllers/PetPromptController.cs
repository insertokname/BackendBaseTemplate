using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.PetPrompts;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PetPromptsController : EntityController<PetPrompt, CreatePetPromptCommand>
{
    private readonly GetRandomQueryHandler<PetPrompt> _getRandomQueryHandler;
    public PetPromptsController(
        CreateCommandHandler<CreatePetPromptCommand, PetPrompt> createHandler,
        DeleteByIdCommandHandler<PetPrompt> deleteHandler,
        GetAllQueryHandler<PetPrompt> getAllHandler,
        GetRandomQueryHandler<PetPrompt> getRandomQueryHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomQueryHandler = getRandomQueryHandler;
    }

    [HttpGet]
    [Route("random")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<string>> Random()
    {
        return Ok(
            (await _getRandomQueryHandler.HandleAsync()).Prompt
        );
    }
}
