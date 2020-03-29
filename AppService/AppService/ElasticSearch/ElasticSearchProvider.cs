using System;
using AppService.ElasticSearch;
using Domain.Model;
using Microsoft.Extensions.Options;
using Nest;

namespace AppService.ElasticSearch
{
    public class ElasticSearchProvider: IElasticProvider
    {
        public IElasticClient ElasticClient { get; }

        public ElasticSearchProvider(IOptions<AppSettings> appSettingsOptions)
        {
            var appSettings = appSettingsOptions.Value;
            var url = appSettings.ElasticURL;
            var defaultIndex = appSettings.DefaultIndex;

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

            ElasticClient = new ElasticClient(settings);
        }
    }
}