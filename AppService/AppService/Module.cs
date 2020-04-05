using System.Collections.Generic;
using AppService.Commands;
using AppService.Commands.Handlers;
using AppService.Decorators.Command;
using AppService.ElasticSearch;
using AppService.Interfaces;
using AppService.MQ;
using AppService.Providers.Imlementation;
using AppService.Providers.Implementation;
using AppService.Providers.Interfaces;
using AppService.Providers.mplementation;
using AppService.Queries;
using AppService.Queries.Handlers;
using Domain.Model;

namespace AppService
{
    public static class Module
    {
        public static void Init()
        {
            Repository.Module.Init();
            IoC.Manager.Register<IRabbitMQ,RabbitMQProvider>();
            IoC.Manager.Register<IUserProvider, UserProvider>();
            IoC.Manager.Register<ILocationProvider, LocationProvider>();
            IoC.Manager.Register<IPopulationProvider,PopulationProvider>();
            IoC.Manager.Register<IMediator,ServiceMediator>();
            IoC.Manager.Register<IElasticProvider,ElasticSearchProvider>();
            IoC.Manager.Register<ICommandHandler<AddLocationDescriptionCommand>,AddLocationDescriptionHandler>();
            IoC.Manager.Register<IQueryHandler<AuthenticateQuery,User>,AuthenticateHandler>();
            IoC.Manager.Register<IQueryHandler<GetLocationQuery,Location>,GetLocationHandler>();
            IoC.Manager.Register<IQueryHandler<SearchLocationDescriptionQuery,IEnumerable<Location>>,SearchLocationDescriptionHandler>();
        }
    }
}