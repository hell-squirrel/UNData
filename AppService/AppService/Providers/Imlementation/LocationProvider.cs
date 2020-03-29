using System;
using System.Collections.Generic;
using System.Transactions;
using AppService.ElasticSearch;
using AppService.Providers.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Nest;

namespace AppService.Providers.Imlementation
{
    public class LocationProvider : ILocationProvider
    {
        private readonly IAnalitics _analitics;
        private readonly IElasticClient _elasticClient;
        private readonly AppSettings _appSettings;

        public LocationProvider(IElasticProvider elasticProvider, IAnalitics analitics)
        {
            _analitics = analitics;
            _elasticClient = elasticProvider.ElasticClient;
        }

        public IEnumerable<Location> Search(string query, int page = 1, int pageSize = 5)
        {
            var response = _elasticClient.SearchAsync<Location>(
                s => s.Query(q => q.QueryString(d => d.Query(query)))
                    .From((page - 1) * pageSize)
                    .Size(pageSize)).Result;
            return response.Documents;
        }

        public void SaveLocation(int locationId, string locationName)
        {
            this._analitics.SaveLocation(locationId, locationName);
        }

        public Location AddDescription(int locationId, string description)
        {
            using (var scope = new TransactionScope(
                TransactionScopeOption.Required))
            {
                var location = this._analitics.AddDescription(locationId, description);
                var indexing = this._elasticClient.IndexDocument(location);
                if (!indexing.IsValid)
                {
                    throw new ApplicationException("Cannot index location");
                }

                scope.Complete();
                return location;
            }
        }

        public Location Get(int locationId)
        {
            return this._analitics.GetLocation(locationId);
        }
    }
}