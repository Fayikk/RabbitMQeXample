using ForExampleRabbitMQ.Entities;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ForExampleRabbitMQ.Repo;
using ForExampleRabbitMQ.RabbitMQSender;
using ForExampleRabbitMQ.RabbitForOrder;
using ForExampleRabbitMQ.RabbitForPayment;

namespace ForExampleRabbitMQ.ForPaymentConsumer
{
    public class RabbitMQPaymentConsumer : BackgroundService
    {
        private readonly PaymentService _paymentService;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly IRabbitMQPaymentMessageSender _messageSender;
        public RabbitMQPaymentConsumer(PaymentService paymentService, IRabbitMQPaymentMessageSender messageSender)
        {
            _paymentService = paymentService;
            _messageSender = messageSender;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: "succesProcessTopic", false, false, false, arguments: null);

        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Order order = JsonConvert.DeserializeObject<Order>(content);
                HandleMessage(order).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("succesProcessTopic", false, consumer);
            return Task.CompletedTask;
        }

        private async Task HandleMessage(Order order)
        {
            Payment payment = new()
            {
               Price = order.Price,
               Name = order.Name,

            };
           await _paymentService.CreatePayment(payment);
            try
            {
                _messageSender.SendMessage(payment);
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}