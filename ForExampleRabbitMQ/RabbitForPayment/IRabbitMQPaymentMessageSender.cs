namespace ForExampleRabbitMQ.RabbitForPayment
{
    public interface IRabbitMQPaymentMessageSender
    {
        void SendMessage(BaseMessage baseMessage);

    }
}
