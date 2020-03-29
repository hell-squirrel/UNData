using System;
using System.Transactions;
using AppService.ElasticSearch;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using Nest;

namespace AppService.Commands.Handlers
{
    public class AddLocationDescriptionHandler:ICommandHandler<AddLocationDescriptionCommand>
    {
        private readonly IAnalitics _analitics;
        private readonly IElasticClient _elasticClient;
        private readonly AppSettings _appSettings;
        
        public AddLocationDescriptionHandler( IAnalitics analitics,IElasticProvider elasticProvider)
        {
            _analitics = analitics;
            _elasticClient = elasticProvider.ElasticClient;
        }
        public void Execute(AddLocationDescriptionCommand command)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required))
            {
                var location = this._analitics.AddDescription(command.LocationId, command.Description);
                var indexing = this._elasticClient.IndexDocument(location);
                if (!indexing.IsValid)
                {
                    throw new ApplicationException("Cannot index location");
                }

                scope.Complete();
            }
        }
    }
}