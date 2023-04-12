using System.ComponentModel.DataAnnotations;

namespace ForExampleRabbitMQ.Entities
{
    public class Payment:BaseMessage
    {
        [Key]
        public int Id { get; set; } 
        public int Price { get; set; } 
        public string Name { get; set; }    
        
    }
}
