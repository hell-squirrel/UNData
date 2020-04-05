using System.Threading;
using System.Threading.Tasks;
using AppService.ElasticSearch;
using AppService.MQ;
using Domain.Configs;
using Domain.Model;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace AppService
{
    public class ElasticSearchService : IHostedService
    {
        private Task _task;
        private readonly IElasticProvider _elasticProvider;
        private readonly ILogger<ElasticSearchService> _logger;
        private readonly IRabbitMQ _rabbitMq;
        private readonly AppSettings _appSettings;

        public ElasticSearchService(IOptions<AppSettings> appsettings, IRabbitMQ rabbitMq,
            IElasticProvider elasticProvider, ILogger<ElasticSearchService> logger)
        {
            _elasticProvider = elasticProvider;
            _logger = logger;
            _rabbitMq = rabbitMq;
            _appSettings = appsettings.Value;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            var taskFactory =
                new TaskFactory(TaskCreationOptions.RunContinuationsAsynchronously,
                    TaskContinuationOptions.LongRunning);
            _task = taskFactory.StartNew(() => { Handle(); }, cancellationToken);

            if (_task.IsCompleted)
            {
                return _task;
            }

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return _task;
        }

        private void Handle()
        {
            var channel = _rabbitMq.GetChannel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (ch, ea) =>
            {
                var body = ea.Body;
                if (!(body.ToArray().ByteArrayToObject() is Location location))
                {
                    return;
                }

                if (ea.DeliveryTag >= 3)
                {
                    _logger.LogError("Can not handle description for " + location.LocationId);
                }
                else
                {
                    var indexing = _elasticProvider.ElasticClient.IndexDocument(location);
                    if (!indexing.IsValid)
                    {
                        channel.BasicNack(ea.DeliveryTag, false, true);
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                }
            };
            channel.BasicConsume(queue: _appSettings.MQName,
                autoAck: false,
                consumer: consumer);
        }
    }
}