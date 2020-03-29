namespace AppService.Interfaces
{
    public interface IMediator
    {
        TResponse Execute<TResponse>(IQuery<TResponse> request);

        void Execute<TCommand>(TCommand command) where TCommand:ICommand;
    }
}