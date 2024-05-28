using System;
namespace business_logic_layer.ViewModel
{
    public class OrderModelAit
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string recipientName { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetailModelAit> OrderDetails { get; set; }

    }

    public class OrderModelAdd
    {
        public Guid UserId { get; set; }
        public ICollection<OrderDetailModelAdd> OrderDetails { get; set; }
    }

    public class GetOrderModelAit
    {
        public Guid OrderId { get; set; }
        public Guid UserId { get; set; }
        public string CustomerEmail { get; set; }
        public string recipientName { get; set; }
        public string city { get; set; }
        public string Street { get; set; }
        public int? huisNumber { get; set; }
        public string postalCode { get; set; }
        public string phoneNumber { get; set; }
        public long OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public ICollection<OrderDetailModelAit> OrderDetails { get; set; }
    }

}

