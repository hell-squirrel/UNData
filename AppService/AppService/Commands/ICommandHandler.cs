namespace AppService.Commands
{
    public interface ICommandHandler<in ICommand>
    {
        void Execute(ICommand command);
    }
}