namespace OZQ_cart.DTOs
{
    public class OrderToReturnDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public virtual ICollection<OrderItemToReturnDto> OrderItems { get; set; }
        public double TotalPrice { get; set; }
    }

}
