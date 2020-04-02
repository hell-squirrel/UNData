namespace Domain.Configs
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string ElasticURL { get; set; }
        public string DefaultIndex { get; set; }

        public string MongoConnectionString { get; set; }
        public string MongoDatabaseName { get; set; }
    }
}