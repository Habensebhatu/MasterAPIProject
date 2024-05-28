using System;
using Data_layer.Context.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context
{
	public class OrderEntiyAit
	{

        [Key]
        public Guid OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public Guid UserId { get; set; }
        public long OrderNumber { get; set; }
        [ForeignKey("UserId")]
        public UserRegistrationEntityModel User { get; set; }
        public ICollection<OrderDetailEntityAit> OrderDetails { get; set; }
    }
}

