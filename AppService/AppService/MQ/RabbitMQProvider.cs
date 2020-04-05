using Domain.Configs;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using RabbitMQ.Client;

namespace AppService.MQ
{
    public class RabbitMQProvider:IRabbitMQ
    {
        private readonly IModel _channelMq;
        public RabbitMQProvider(IOptions<AppSettings> appsettings)
        {
            
            var factory = new ConnectionFactory() { HostName = appsettings.Value.MQHost };
            var connection = factory.CreateConnection();
            var channel = connection.CreateModel();
            channel.QueueDeclare(queue: appsettings.Value.MQName,
                durable: false,
                exclusive: false,
                autoDelete: false,
                arguments: null);
            _channelMq = channel;
        }

        public IModel GetChannel()
        {
            return _channelMq;
        }
    }
}