namespace ForExampleRabbitMQ.RabbitForOrder
{
    public interface IRabbitMQOrderMessageSender
    {
        void SendMessage(BaseMessage baseMessage, String queueName);

    }
}
