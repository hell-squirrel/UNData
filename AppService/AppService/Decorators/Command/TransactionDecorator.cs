using AppService.Commands;
using AppService.Interfaces;

namespace AppService.Decorators.Command
{
    public class TransactionDecorator<THandler>: ICommandHandler<ICommand> where THandler:ICommandHandler<ICommand> 
    {
        private readonly ICommandHandler<ICommand> _decorated;

        public TransactionDecorator(ICommandHandler<ICommand> decorated)
        {
            _decorated = decorated;
        }

        public void Execute(ICommand command)
        {
            _decorated.Execute(command);
        }
    }
}