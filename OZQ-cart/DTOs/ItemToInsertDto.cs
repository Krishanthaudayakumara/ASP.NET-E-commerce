using System.ComponentModel.DataAnnotations;

namespace OZQ_cart.DTOs
{
    public class ItemToInsertDto
    {
        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Description { get; set; }
    }

}
