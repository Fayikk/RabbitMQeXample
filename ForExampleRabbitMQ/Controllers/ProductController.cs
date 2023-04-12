using ForExampleRabbitMQ.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForExampleRabbitMQ.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        DataContext _context;
        public ProductController(DataContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task Create(Product product)
        {
             _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }
    }
}
