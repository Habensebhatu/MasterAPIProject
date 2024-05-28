using Data_layer.Context;
using Data_layer.Context.Data;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class CartDAL
    {
        private readonly DbContext _context;
        public CartDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<CartEnityModel> AddProduct(CartEnityModel cartItem)

        {

            var existingItem = _context.Set<CartEnityModel>().FirstOrDefault(item => item.productId == cartItem.productId && item.Price == cartItem.Price);
            var existingItemm = _context.Set<Product>().FirstOrDefault(item => item.ProductId == cartItem.productId && item.CratePrice == cartItem.Price);

            if (existingItem != null)
            {
                if (existingItemm != null)
                {
                    existingItem.Quantity += cartItem.Quantity;
                }

                else
                {
                    existingItem.Quantity += cartItem.Quantity;
                    _context.Set<CartEnityModel>().Update(existingItem);
                }

            }
            else
            {

                _context.Set<CartEnityModel>().Add(cartItem);
            }
            await _context.SaveChangesAsync();

            return cartItem;


        }

        public Guid GetDefaultUserId()
        {
            return _context.Set<UserRegistrationEntityModel>().FirstOrDefault(u => u.Email == "default@guest.com").UserId;
        }


        public async Task<List<CartEnityModel>> GetCartItems(string sessionId, Guid? userId = null)
        {
            
            if (userId.HasValue && userId.Value != Guid.Empty)
                return await _context.Set<CartEnityModel>().Where(item => item.UserId == userId.Value).ToListAsync();
            else
                return await _context.Set<CartEnityModel>().Where(item => item.SessionId == sessionId).ToListAsync();
        }

        public async Task ClearCart(string sessionId, Guid? userId)
        {
            
            List<CartEnityModel> RemoveCart;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                RemoveCart = await _context.Set<CartEnityModel>().Where(item => item.UserId == userId.Value).ToListAsync();
            }
            else
            {
                RemoveCart = await _context.Set<CartEnityModel>().Where(item => item.SessionId == sessionId).ToListAsync();
            }
            _context.Set<CartEnityModel>().RemoveRange(RemoveCart);
            await _context.SaveChangesAsync();
        }



        public async Task RemoveProduct(Guid productId, Guid? userId, string sessionId)
        {
            CartEnityModel productToRemove = null;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                productToRemove = _context.Set<CartEnityModel>().FirstOrDefault(item => item.cartId == productId && item.UserId == userId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(sessionId))
            {
                productToRemove = _context.Set<CartEnityModel>().FirstOrDefault(item => item.cartId == productId && item.SessionId == sessionId);
            }

            if (productToRemove != null)
            {
                _context.Set<CartEnityModel>().Remove(productToRemove);
                await _context.SaveChangesAsync();
            }
        }


        public async Task<CartEnityModel> UpdateProductQuantity(Guid productId, Guid? userId, string sessionId)
        {
            CartEnityModel productToUpdate = null;

            if (userId.HasValue && userId.Value != Guid.Empty)
            {
                productToUpdate = _context.Set<CartEnityModel>().FirstOrDefault(item => item.productId == productId && item.UserId == userId.Value);
            }
            else if (!string.IsNullOrWhiteSpace(sessionId))
            {
                productToUpdate = _context.Set<CartEnityModel>().FirstOrDefault(item => item.productId == productId && item.SessionId == sessionId);
            }

            if (productToUpdate != null)
            {
                productToUpdate.Quantity += -1;
              

                if (productToUpdate.Quantity <= 0)
                {
                    _context.Set<CartEnityModel>().Remove(productToUpdate);
                }
                else
                {
                    _context.Set<CartEnityModel>().Update(productToUpdate);
                }

                await _context.SaveChangesAsync();
            }

            return productToUpdate;  
        }
    }
}

