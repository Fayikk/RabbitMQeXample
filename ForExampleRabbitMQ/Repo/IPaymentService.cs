using ForExampleRabbitMQ.Entities;

namespace ForExampleRabbitMQ.Repo
{
    public interface IPaymentService
    {
        Task<bool> CreatePayment(Payment payment);
    }
}
