using Microsoft.AspNetCore.Connections;
//using static ForExampleRabbitMQ.RabbitMQSender.RabbitMQMessageSender;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ForExampleRabbitMQ.RabbitForPayment
{
    public class RabbitMQPaymentMessageSender : IRabbitMQPaymentMessageSender
    {
        
            private readonly string _hostName;
            private readonly string _password;
            private readonly string _username;
            private IConnection _connection;
        private const string ExchangeName = "PublishSubscribePaymentUpdate_Exchange";
        public RabbitMQPaymentMessageSender()
            {
                _hostName = "localhost";
                _password = "guest";
                _username = "guest";
            }

        public void SendMessage(BaseMessage message)
        {
            if (ConnectionExists())
            {
                using var channel = _connection.CreateModel();
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Fanout, durable: false);
                var json = JsonConvert.SerializeObject(message);
                var body = Encoding.UTF8.GetBytes(json);
                channel.BasicPublish(exchange: ExchangeName, "", basicProperties: null, body: body);
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostName,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception)
            {
                //log exception
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }
            CreateConnection();
            return _connection != null;
        }
    }
}
