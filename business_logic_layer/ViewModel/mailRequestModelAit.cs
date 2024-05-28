using System;
namespace business_logic_layer.ViewModel
{
	public class mailRequestModelAit
	{
        public string recipientName { get; set; }
        public string Email { get; set; }
        public string city { get; set; }
        public string adres { get; set; }
        public string postalCode { get; set; }
        public DateTime OrderDate { get; set; }
        public long OrderNummer { get; set; }
        public List<OrderItemModelAit> OrderItems { get; set; } = new List<OrderItemModelAit>();
    }

    public class OrderItemModelAit
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Total { get; set; }
        public string ImageUrl { get; set; }
    }
}

