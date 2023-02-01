using System.ComponentModel.DataAnnotations;

namespace OZQ_cart.Models
{
    public class Feedback
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        [Required]
        public string Comment { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
