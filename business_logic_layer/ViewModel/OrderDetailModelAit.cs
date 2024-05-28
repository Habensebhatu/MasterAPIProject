using System;
namespace business_logic_layer.ViewModel
{
    public class OrderDetailModelAit
    {
        public Guid OrderDetailId { get; set; }
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public int contents { get; set; }
        public decimal AmountTotal { get; set; }
    }

    public class OrderDetailModelAdd
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal AmountTotal { get; set; }
        public int contents { get; set; }
        public decimal Price { get; set; }
    }

}