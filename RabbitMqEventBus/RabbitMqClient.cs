using Newtonsoft.Json;
using Polly;
using Polly.Retry;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Net.Sockets;
using System.Text;

namespace RabbitMqEventBus
{
    public class RabbitMqClient : IEventBus
    {
        const string BROKER_NAME = "rabbitExchange";
        const string _queueName = "rabbitQueue";
        private readonly IConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IEventBusSubscriptionsManager _subsManager;

        public RabbitMqClient(IEventBusSubscriptionsManager subsManager)
        {
            _factory = new ConnectionFactory() { HostName = "rabbitmq", UserName = "user", Password = "password", VirtualHost = "/"};
            _connection = _factory.CreateConnection();
            using (var channel = _connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");
                channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            }
                this._subsManager = subsManager;
        }
        public void Publish(IntegrationEvent @event)
        {
            var policy = Policy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(5, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));
            using (var channel = _connection.CreateModel())
            {
                var eventName = @event.GetType().Name;              
                string message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                policy.Execute(() =>
                {              
                    channel.BasicPublish(exchange: BROKER_NAME,
                        routingKey: eventName,
                        basicProperties: null,
                        body: body);
                });
            }
        }

        public void Subscribe<T, TH>()
        where T : IntegrationEvent
        where TH : IIntegrationEventHandler<T>
        {
            var policy = Policy.Handle<BrokerUnreachableException>()
                    .Or<SocketException>()
                    .WaitAndRetry(5, retryAttemp => TimeSpan.FromSeconds(Math.Pow(2, retryAttemp)));
            var eventName = _subsManager.GetEventKey<T>();

            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                using (var channel = _connection.CreateModel())
                {                   
                    channel.QueueBind(queue: _queueName,
                                        exchange: BROKER_NAME,
                                        routingKey: eventName);
                }
            }

            _subsManager.AddSubscription<T, TH>();
        }
    }
}
