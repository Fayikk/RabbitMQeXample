using ForExampleRabbitMQ.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForExampleRabbitMQ.Repo
{
    public class PaymentService : IPaymentService
    {
        DbContextOptions<DataContext> _dbContext;

        public PaymentService(DbContextOptions<DataContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreatePayment(Payment payment)
        {
            await using var _db = new DataContext(_dbContext);
            _db.Payments.Add(payment);
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
