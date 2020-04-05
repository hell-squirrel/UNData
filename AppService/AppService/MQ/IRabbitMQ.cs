using RabbitMQ.Client;

namespace AppService.MQ
{
    public interface IRabbitMQ
    {
        IModel GetChannel();
    }
}