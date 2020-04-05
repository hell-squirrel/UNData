using System;
using System.Text;
using System.Transactions;
using AppService.ElasticSearch;
using AppService.MQ;
using Domain.Configs;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using Nest;
using RabbitMQ.Client;

namespace AppService.Commands.Handlers
{
    public class AddLocationDescriptionHandler:ICommandHandler<AddLocationDescriptionCommand>
    {
        private readonly IAnalitics _analitics;
        private readonly IElasticClient _elasticClient;
        private readonly IRabbitMQ _rabbitMq;
        private readonly AppSettings _appSettings;
        
        public AddLocationDescriptionHandler(IAnalitics analitics,IElasticProvider elasticProvider,IRabbitMQ rabbitMq,IOptions<AppSettings> appsettings)
        {
            _analitics = analitics;
            _elasticClient = elasticProvider.ElasticClient;
            _rabbitMq = rabbitMq;
            _appSettings = appsettings.Value;
        }
        public void Execute(AddLocationDescriptionCommand command)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required))
            {
                var location = this._analitics.AddDescription(command.LocationId, command.Description);

                var channel = _rabbitMq.GetChannel();
                var payload = location.ObjectToByteArray();

                var props = channel.CreateBasicProperties();
                props.DeliveryMode = 2;
                channel.BasicPublish(exchange: "",
                    routingKey: _appSettings.MQName,
                    basicProperties: props,
                    body: payload);
                
                scope.Complete();
            }
        }
    }
}