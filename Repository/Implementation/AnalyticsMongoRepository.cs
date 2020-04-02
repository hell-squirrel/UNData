using System.Collections.Generic;
using Domain.Configs;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Repository.Implementation
{
    public class AnalyticsMongoRepository: IAnalitics
    {
        private readonly AppSettings _appSettings;
        private readonly IMongoCollection<Location> _locationCollection;
        private readonly IMongoCollection<Population> _populationCollection;
        public AnalyticsMongoRepository(IOptions<AppSettings> appSettingsOptions)
        {
            _appSettings = appSettingsOptions.Value;
            var client = new MongoClient(_appSettings.MongoConnectionString);
            var db  = client.GetDatabase(_appSettings.MongoDatabaseName);

            _locationCollection = db.GetCollection<Location>(nameof(Location));
            _populationCollection = db.GetCollection<Population>(nameof(Population));

        }
        public IList<Population> GetData(int location)
        {
            return _populationCollection.Find<Population>(p => p.LocationId == location).ToList();
        }

        public void SavePopulation(IEnumerable<Population> dataset)
        {
            _populationCollection.InsertMany(dataset);
        }

        public void SaveLocation(int locationId, string name)
        {
            _locationCollection.InsertOne(new Location(){Name = name,LocationId = locationId});
        }

        public Location GetLocation(int locationId)
        {
            return _locationCollection.Find<Location>(l => l.LocationId == locationId).FirstOrDefault();
        }

        public Location AddDescription(int locationId, string description)
        {
            var filter = Builders<Location>.Filter.Eq(nameof(Location.LocationId), locationId);
            var update = Builders<Location>.Update.Set(nameof(Location.Description), description);
           
            var result = _locationCollection.FindOneAndUpdate(filter, update);
            return result;
        }
    }
}