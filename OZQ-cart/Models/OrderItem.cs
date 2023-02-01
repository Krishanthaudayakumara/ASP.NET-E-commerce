using System.Text.Json.Serialization;

namespace OZQ_cart.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public virtual Item Item { get; set; }
        public int Quantity { get; set; }

        public int OrderId { get; set; }

        [JsonIgnore]
        public Order Order { get; set; }
        public int UnitPrice { get; internal set; }
    }

}
