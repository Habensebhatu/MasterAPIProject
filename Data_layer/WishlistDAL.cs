using Data_layer.Context;
using Data_layer.Context.Data;
using Microsoft.EntityFrameworkCore;



namespace Data_layer
{
    public class WishlistDAL
    {
        private readonly DbContext _context;
        public WishlistDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<WishlistEntityModel> CreateWishlistForUser(Guid userId)
        {
            var userWishlist = new WishlistEntityModel
            {
                WishlistId = Guid.NewGuid(),
                UserId = userId
            };

            _context.Set<WishlistEntityModel>().Add(userWishlist);
            await _context.SaveChangesAsync();

            return userWishlist;
        }

        public WishlistEntityModel GetWishlistByUserId(Guid userId)
        {
            return _context.Set<WishlistEntityModel>().FirstOrDefault(w => w.UserId == userId);
        }

        public async Task AddProductToWishlist(ProductWishlist productWishlistItem)
        {
            _context.Set<ProductWishlist>().Add(productWishlistItem);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>> GetProductsInWishlist(Guid userId)
        {
            return await _context.Set<ProductWishlist>()
                .Include(pw => pw.Product)
                .ThenInclude(p => p.Category)
                .Include(pw => pw.Product.ProductImages)
                .Where(pw => pw.Wishlist.UserId == userId)
                .Select(pw => pw.Product)
                .ToListAsync();
        }

        public async Task<bool> DeleteProductFromWishlist(Guid productId, Guid userId)
        {
            var productWishlistItem = _context.Set<ProductWishlist>()
                .FirstOrDefault(pw => pw.ProductId == productId && pw.Wishlist.UserId == userId);

            if (productWishlistItem == null) return false;

            _context.Set<ProductWishlist>().Remove(productWishlistItem);
            await _context.SaveChangesAsync();
            return true;
        }



    }
}

