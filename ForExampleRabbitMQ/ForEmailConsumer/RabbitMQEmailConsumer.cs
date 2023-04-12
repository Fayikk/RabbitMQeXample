using ForExampleRabbitMQ.Entities;
using ForExampleRabbitMQ.RabbitForPayment;
using ForExampleRabbitMQ.Repo;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using Newtonsoft.Json;
using ForExampleRabbitMQ.RabbitForEmail;
using ForExampleRabbitMQ.Helper;

namespace ForExampleRabbitMQ.ForEmailConsumer
{
    public class RabbitMQEmailConsumer : BackgroundService
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private const string EchangeName = "PublishSubscribePaymentUpdate_Exchange";
        private readonly EmailSender _emailSender;
        string queueName = "";
        public RabbitMQEmailConsumer(EmailSender emailSender)
        {
            _emailSender = emailSender;
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest",
            };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(EchangeName, ExchangeType.Fanout);
            queueName = _channel.QueueDeclare().QueueName;
        }



        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (ch, ea) =>
            {
                var content = Encoding.UTF8.GetString(ea.Body.ToArray());
                Payment payment = JsonConvert.DeserializeObject<Payment>(content);
                HandleMessage(payment).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume("succesProcessTopic", false, consumer);
            return Task.CompletedTask;
        }

        private async Task HandleMessage(Payment payment)
        {
            try
            {
                await _emailSender.SendEmailAsync("veznedaroglufayik2@gmail.com", "denee", "denememe"); ;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
