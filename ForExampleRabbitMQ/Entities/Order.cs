using System.ComponentModel.DataAnnotations;

namespace ForExampleRabbitMQ.Entities
{
    public class Order:BaseMessage
    {
        [Key]
        public int Id { get; set; } 
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
    }
}
