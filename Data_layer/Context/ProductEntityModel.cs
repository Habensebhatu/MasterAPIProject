using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data_layer.Context
{
    public class Product
    {
        [Key]
        public Guid ProductId { get; set; }
        public string Title { get; set; }
        public decimal PiecePrice { get; set; }
        public decimal? Kilo { get; set; }
        public int? InstokeOfPiece { get; set; }

        public decimal? CratePrice { get; set; }
        public int? CrateQuantity { get; set; }
        public int? InstokeOfCrate { get; set; }

        public string Description { get; set; }
        public bool IsPopular { get; set; } = false;
        public Guid CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ProductImageEnityModel> ProductImages { get; set; }
        public virtual ICollection<ProductWishlist> ProductWishlists { get; set; }

    }
}
