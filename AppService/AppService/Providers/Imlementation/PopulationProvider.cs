using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using AppService.Providers.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.Extensions.Options;
using Nest;
using Newtonsoft.Json.Linq;

namespace AppService.Providers.mplementation
{
    public class PopulationProvider : IPopulationProvider
    {
        private const string TimeFormat = "yyyy-MM-dd";
        private readonly IAnalitics _analitics;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IElasticClient _elasticClient;
        private readonly ILocationProvider _locationProvider;

        public PopulationProvider(IAnalitics analitics, ILocationProvider locationProvider,IHttpClientFactory clientFactory)
        {
            _analitics = analitics;
            _locationProvider = locationProvider;
            _clientFactory = clientFactory;
        }

        public IList<Population> GetData(int location)
        {
            return this._analitics.GetData(location);
        }

        public void LoadPopulation(DateTime startData,DateTime endDate)
        {
            var dataset = this.LoadDataFromUNData(startData,endDate).Result;
            if (dataset == null)
            {
                throw new Exception("Cannot load data");
            }

            var location = this.GetLocationId(dataset);
            var population = this.ParsePopulation(dataset);
            population = population.Select(x => this.MapPopulationWithLocation(x, 2));
            
            var locationId = this.GetLocationId(location);
            var locationName = this.GetLocationName(location);
            
            this._locationProvider.SaveLocation(locationId,locationName);
            
            population = population.Select(x =>MapPopulationWithLocation(x,locationId));
            this._analitics.SavePopulation(population);
        }

        private JToken GetLocationId(JObject obj)
        {
            var structure = obj.GetValue("structure").SelectToken("dimensions").SelectToken("series");
            var location = structure.Where(y => ((JProperty) y.First).Value.ToString() == "REF_AREA").First()
                .SelectToken("values");
            return location;
        }

        private IEnumerable<Population> ParsePopulation(JObject jobj)
        {
            var result = new List<Population>();
            foreach (var set in jobj.SelectTokens("dataSets"))
            {
                var series = set.First.SelectToken("series");
                var observations = series.SelectToken("0:0:0:0:0:0:0").SelectToken("observations");
                var collections = observations.Select(x =>
                {
                    var data = x.First;
                    return new Population
                    {
                        Pointer = Guid.NewGuid(),
                        Indicator = data[0].Value<int>(),
                        Frequency = data[1].Value<int>(),
                        Age = data[2].Value<int>(),
                        Sex = data[3].Value<int>(),
                    };
                });
                result.AddRange(collections);
            }

            return result;
        }

        private Population MapPopulationWithLocation(Population population, int location)
        {
            population.LocationId = location;
            return population;
        }

        public int GetLocationId(JToken location)
        {
            return location.First.SelectToken("id").ToObject<int>();
        }

        public string GetLocationName(JToken location)
        {
            return location.First.SelectToken("name").ToObject<string>();
        }

        private async Task<JObject> LoadDataFromUNData(DateTime startData,DateTime endDate)
        {
            var start = startData.ToString(TimeFormat);
            var end = endDate.ToString(TimeFormat);
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://data.un.org/ws/rest/data/UNSD,DF_UNDATA_WPP,2.0/SP_POP_TOTL.._T._T..804.NC/ALL/?detail=full&startPeriod={start}&endPeriod={end}&dimensionAtObservation=TIME_PERIOD");
            request.Headers.Add("Accept", "text/json");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var responseStream = await response.Content.ReadAsStringAsync();
                var jObj = JObject.Parse(responseStream);
                return jObj;
            }
            else
            {
                return null;
            }
        }
    }
}