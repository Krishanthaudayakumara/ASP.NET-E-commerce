namespace OZQ_cart.Models
{
    public class CartItem
    {
        public int Id { get; set; }

        public int ItemId { get; set; }

        public int Quantity { get; set; }

        public int CustomerId { get; set; }
       
    }
}
