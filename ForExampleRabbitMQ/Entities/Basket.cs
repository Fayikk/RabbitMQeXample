using System.ComponentModel.DataAnnotations;

namespace ForExampleRabbitMQ.Entities
{
    public class Basket:BaseMessage
    {
        [Key]
        public int BasketId { get; set; }   
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
