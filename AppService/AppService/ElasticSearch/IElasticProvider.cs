using Nest;

namespace AppService.ElasticSearch
{
    public interface IElasticProvider
    {
        public IElasticClient ElasticClient { get; }
    }
}