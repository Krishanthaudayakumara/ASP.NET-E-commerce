namespace OZQ_cart.DTOs
{
    public class OrderToInsertDto
    {
        public int CustomerId { get; set; }
        public virtual ICollection<OrderItemToInsertDto> OrderItems { get; set; }
    }

}
