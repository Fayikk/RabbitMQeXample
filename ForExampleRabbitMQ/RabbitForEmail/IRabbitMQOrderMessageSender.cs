namespace ForExampleRabbitMQ.RabbitForEmail
{
    public interface IRabbitMQEmailMessageSender
    {
        void SendMessage(BaseMessage baseMessage, String queueName);

    }
}
