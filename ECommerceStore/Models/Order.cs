using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace ECommerceStore.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string CustomerName { get; set; }

        public string CustomerEmail { get; set; }

        public string ShippingAddress { get; set; }

        public DateTime OrderDate { get; set; }

        public decimal TotalAmount { get; set; }

        public string Status { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }
    }
}
