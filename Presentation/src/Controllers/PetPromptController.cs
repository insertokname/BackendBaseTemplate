using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.PetPrompts;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.application.Query.PetPrompts;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PetPromptsController : EntityController<PetPrompt, CreatePetPromptCommand>
{
    private readonly GetRandomPromptQueryHandler _getRandomPromptQueryHandler;
    public PetPromptsController(
        CreateCommandHandler<CreatePetPromptCommand, PetPrompt> createHandler,
        DeleteByIdCommandHandler<PetPrompt> deleteHandler,
        GetAllQueryHandler<PetPrompt> getAllHandler,
        GetRandomPromptQueryHandler getRandomPetPromptQueryHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
        _getRandomPromptQueryHandler = getRandomPetPromptQueryHandler;
    }

    [HttpGet]
    [Route("random")]
    public async Task<ActionResult<PetPrompt>> Random()
    {
        return Ok(await _getRandomPromptQueryHandler.HandleAsync());
    }
}
