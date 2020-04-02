using System;
using System.Collections.Generic;
using Domain.Configs;
using Domain.Model;
using Microsoft.Extensions.Options;
using Nest;

namespace AppService.Queries.Handlers
{
    public class SearchLocationDescriptionHandler :IQueryHandler<SearchLocationDescriptionQuery,IEnumerable<Location>>
    {
        private readonly IElasticClient _elasticClient;
        private readonly AppSettings _appSettings;
        
        public SearchLocationDescriptionHandler(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
            var url = this._appSettings.ElasticURL;
            var defaultIndex = this._appSettings.DefaultIndex;

            var settings = new ConnectionSettings(new Uri(url))
                    .PrettyJson()
                    .DisableDirectStreaming()
                    .DefaultIndex(defaultIndex)
                    .DefaultMappingFor<Location>(m => m
                        .IdProperty(p => p.LocationId)
                        .PropertyName(p => p.Name, "Name")
                        .PropertyName(p => p.LocationId, "LocationId")
                        .PropertyName(p => p.Description, "Description"))
                ;

            _elasticClient = new ElasticClient(settings);
        }
        public IEnumerable<Location> Execute(SearchLocationDescriptionQuery query)
        {
            var response = _elasticClient.SearchAsync<Location>(
                s => s.Query(q => q.QueryString(d => d.Query(query.Query)))
                    .From((query.Page - 1) * query.PageSize)
                    .Size(query.PageSize)).Result;
            return response.Documents;
        }
    }
}