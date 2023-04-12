using ForExampleRabbitMQ.Entities;
using ForExampleRabbitMQ.RabbitMQSender;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForExampleRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        DataContext _context;
        private readonly IRabbitMQMessageSender _messsageSender;
        public BasketController(DataContext context,IRabbitMQMessageSender messageSender)
        {
            _messsageSender = messageSender;
            _context = context;
        }

        [HttpPost]
        public async Task Create(int id)
        {
            var product = await _context.Products.FindAsync(id);
            Basket basket = new Basket();
            basket.ProductId = id;
            basket.Id = id;
            basket.Description = product.Description;
            basket.Price = product.Price;
            basket.Name= product.Name;
            _context.Baskets.Add(basket);
            _messsageSender.SendMessage(basket, "checkoutqueue");

            await _context.SaveChangesAsync();
        }
    }
}
