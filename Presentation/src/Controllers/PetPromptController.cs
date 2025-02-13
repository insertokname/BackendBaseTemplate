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
    private readonly GetRandomHandler<PetPrompt> _getRandomHandler;
    public PetPromptsController(
        CreateHandler<CreatePetPromptCommand, PetPrompt> createHandler,
        DeleteByIdHandler<PetPrompt> deleteHandler,
        GetAllHandler<PetPrompt> getAllHandler,
        GetRandomHandler<PetPrompt> getRandomHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomHandler = getRandomHandler;
    }

    [HttpGet]
    [Route("random")]
    [EnableRateLimiting("UnauthorizedEndpointRateLimiter")]
    public async Task<ActionResult<string>> Random()
    {
        return Ok(
            (await _getRandomHandler.HandleAsync()).Prompt
        );
    }
}
