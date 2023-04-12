using ForExampleRabbitMQ.Entities;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ForExampleRabbitMQ.Repo;
using ForExampleRabbitMQ.RabbitMQSender;
using ForExampleRabbitMQ.RabbitForOrder;

namespace ForExampleRabbitMQ.Consumer
{
    public class RabbitMQConsumer : BackgroundService
    {
        private readonly OrderServ _orderServ;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IRabbitMQOrderMessageSender _messageSender;
        public RabbitMQConsumer(OrderServ orderServ, IRabbitMQOrderMessageSender messageSender)
        {
            _orderServ = orderServ;
            _messageSender = messageSender;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "checkoutqueue", false, false, false, arguments: null);

        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Basket basket = JsonConvert.DeserializeObject<Basket>(content);
                HandleMessage(basket).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("checkoutqueue", false, consumer);
            return Task.CompletedTask;
        }

        private async Task HandleMessage(Basket basket)
        {
            Order order = new()
            {
                Price = basket.Price,
                Name = basket.Name,
                Description = basket.Description,

            };
           await _orderServ.CreateOrder(order);
            try
            {
                _messageSender.SendMessage(order, "succesProcessTopic");
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}