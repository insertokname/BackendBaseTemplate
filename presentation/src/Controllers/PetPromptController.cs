using System.Collections.Generic;
using System.Threading.Tasks;
using BackendOlimpiadaIsto.application.Commands;
using BackendOlimpiadaIsto.application.Commands.GenericCommands;
using BackendOlimpiadaIsto.application.Commands.PetPrompts;
using BackendOlimpiadaIsto.application.Exceptions;
using BackendOlimpiadaIsto.application.Query;
using BackendOlimpiadaIsto.application.Query.GenericQueries;
using BackendOlimpiadaIsto.domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace BackendOlimpiadaIsto.presentation.Controllers;
[ApiController]
[Route("api/[controller]")]
public class PetPromptsController : EntityController<PetPrompt, CreatePetPromptCommand>
{
    public PetPromptsController(
        CreateCommandHandler<CreatePetPromptCommand, PetPrompt> createHandler,
        DeleteByIdCommandHandler<PetPrompt> deleteHandler,
        GetAllQueryHandler<PetPrompt> getAllHandler
    ) : base(createHandler, deleteHandler, getAllHandler)
    {
    }
}
