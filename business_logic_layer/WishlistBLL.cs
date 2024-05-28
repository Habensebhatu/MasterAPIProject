using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Data_layer.Context.Data;

namespace business_logic_layer
{
    public class WishlistBLL
    {

        private readonly IDbContextFactory _dbContextFactory;
        public WishlistBLL(IDbContextFactory dbContextFactory)
        {
            //_wishlistDAL = new WishlistDAL();
            _dbContextFactory = dbContextFactory;
        }

        public async Task<WishlistModel> AddProductToWishlist(Guid productId, string userID, string connectionStringName)
        {
            var wishlistDAL = new WishlistDAL(_dbContextFactory, connectionStringName);
            if (!Guid.TryParse(userID, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            var userWishlist = wishlistDAL.GetWishlistByUserId(parsedUserId);

            if (userWishlist == null)
            {
                // Create a new wishlist for the user.
                userWishlist = await wishlistDAL.CreateWishlistForUser(parsedUserId);

            }
         
            var productWishlistItem = new Data_layer.Context.ProductWishlist
            {
                ProductWishlistId = Guid.NewGuid(),
                WishlistId = userWishlist.WishlistId,
                ProductId = productId
            };
            await wishlistDAL.AddProductToWishlist(productWishlistItem);
            WishlistModel formaat = new WishlistModel()
            {
                UserId = parsedUserId,
                WishlistId = userWishlist.WishlistId
            };
            return formaat;
        }

        public async Task<List<productModelS>> GetWishlistProducts(string userId, string connectionStringName)
        {
            var wishlistDAL = new WishlistDAL(_dbContextFactory, connectionStringName);
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            var products = await wishlistDAL.GetProductsInWishlist(parsedUserId);

            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Description = p.Description,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl
                    }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }

        public async Task<bool> DeleteProductFromWishlist(Guid productId, string userId, string connectionStringName)
        {
            var wishlistDAL = new WishlistDAL(_dbContextFactory, connectionStringName);
            if (!Guid.TryParse(userId, out var parsedUserId))
            {
                throw new ArgumentException("Invalid userId");
            }

            return await wishlistDAL.DeleteProductFromWishlist(productId, parsedUserId);
        }


    }
}

