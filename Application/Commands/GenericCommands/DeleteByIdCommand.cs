namespace Application.Commands.GenericCommands
{
    public class DeleteByIdCommand
    {
        public required Guid EntityId { get; set; }
    };
}