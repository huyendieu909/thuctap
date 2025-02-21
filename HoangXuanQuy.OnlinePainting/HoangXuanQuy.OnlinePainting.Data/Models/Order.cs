using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HoangXuanQuy.OnlinePainting.Data.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }

        [ForeignKey("Customer")]
        public string? CustomerId { get; set; }
        public virtual Customer? Customer { get; set; }

        //[ForeignKey("Painting")]
        public int PaintingId { get; set; }
        public virtual Painting? Painting { get; set; }

        [DisplayName("Order date")]
        public DateTime OrderDate { get; set; }

        [DisplayName("Total price")]
        public decimal TotalPrice { get; set; }

        [Required]
        [MaxLength(50)]
        [DisplayName("Payment method")]
        public string PaymentMethod { get; set; } = null!;

        [Required]
        [MaxLength(50)]
        [DisplayName("Order status")]
        public string OrderStatus { get; set; } = null!;

        [DisplayName("Shipping address")]
        public string? ShippingAddress { get; set; }

        [DisplayName("Order note")]
        public string? OrderNote { get; set; }
    }
}
