using System.ComponentModel.DataAnnotations;

namespace OZQ_cart.DTOs
{
    public class FeedbackToInsertDto
    {
        public int CustomerId { get; set; }
        [Required]
        public string Comment { get; set; }
    }

}
