using System;
using System.Linq;
using System.Reflection;
using AppService.Commands;
using AppService.Interfaces;
using AppService.Queries;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.Extensions.DependencyInjection;

namespace AppService
{
    public class ServiceMediator : IMediator
    {
        private readonly IServiceProvider serviceProvider;

        public ServiceMediator(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public TResponse Execute<TResponse>(IQuery<TResponse> request)
        {
            var requestedType = typeof(IQueryHandler<,>);
            var handlerType = requestedType.MakeGenericType(request.GetType(), typeof(TResponse));
            var executor = this.serviceProvider.GetService(handlerType);
            var thisType = executor.GetType();
            var theMethod = thisType.GetMethods().First();
            var result = theMethod.Invoke(executor, new[] {request});
            return (TResponse) result;
        }

        public void Execute<TCommand>(TCommand command) where TCommand:ICommand
        {
            var handler =
                this.serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;
            handler.Execute(command);
        }
    }
}