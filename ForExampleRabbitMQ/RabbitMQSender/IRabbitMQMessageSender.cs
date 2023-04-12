namespace ForExampleRabbitMQ.RabbitMQSender
{
    public interface IRabbitMQMessageSender
    {
        void SendMessage(BaseMessage baseMessage, String queueName);

    }
}
