using ForExampleRabbitMQ.Entities;
using Microsoft.EntityFrameworkCore;

namespace ForExampleRabbitMQ.Repo
{
    public class OrderServ : IOrderServ
    {
        DbContextOptions<DataContext> _dbContext;
        public OrderServ(DbContextOptions<DataContext> dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CreateOrder(Order order)
        {
            await using var _db = new DataContext(_dbContext);
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();  
            return true;    
        }
    }
}
