using ForExampleRabbitMQ.Entities;

namespace ForExampleRabbitMQ.Repo
{
    public interface IOrderServ
    {
        Task<bool> CreateOrder(Order order);
    }
}
