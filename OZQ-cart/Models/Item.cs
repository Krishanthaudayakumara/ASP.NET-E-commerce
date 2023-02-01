using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace OZQ_cart.Models
{

    public class Item
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }

        public string Description { get; set; }

        public string Category { get; set; }

        public string Image { get; set; }
    }

}
