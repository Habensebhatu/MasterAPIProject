using System;
using Data_layer.Context.Data;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data_layer.Context
{
	public class ProductWishlist
	{
        [Key]
        public Guid ProductWishlistId { get; set; }

        public Guid WishlistId { get; set; }
        public Guid ProductId { get; set; }

        [ForeignKey("WishlistId")]
        public WishlistEntityModel Wishlist { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }
    }
}

