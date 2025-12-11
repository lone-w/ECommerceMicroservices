using Order.API.Enums;
using System.Collections.Generic;

namespace Order.API.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string ShippingAddress { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = new();
    }
}
