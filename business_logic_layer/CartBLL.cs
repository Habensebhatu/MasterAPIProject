using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context.Data;

namespace business_logic_layer
{
    public class CartBLL
    {

        private readonly IDbContextFactory _dbContextFactory;

        public CartBLL(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<CartModel> AddCart(CartModel cart, string sessionId, string userID, string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            CartEnityModel cartFormat = new CartEnityModel()
            {
                productId = cart.productId,
                Title = cart.Title,
                Price = cart.Price,
                Kilo = cart.Kilo,
                Description = cart.Description,
                ImageUrl = cart.ImageUrl,
                Quantity = cart.Quantity,
                CategoryName = cart.CategoryName,
                SessionId = sessionId,
                UserId = userId
            };

            await cartDAL.AddProduct(cartFormat);
            return cart;
        }

        public string GetDefaultUserId(string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            return cartDAL.GetDefaultUserId().ToString();
        }


        public async Task<List<CartModel>> GetCart(string sessionId, string? userID, string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);
            var cartEntities = await cartDAL.GetCartItems(sessionId, userId);
            return cartEntities.Select(item => new CartModel
            {
                productId = item.productId,
                cartId = item.cartId,
                Title = item.Title,
                Price = item.Price,
                Description = item.Description,
                ImageUrl = item.ImageUrl,
                Kilo = item.Kilo,
                Quantity = item.Quantity,
                CategoryName = item.CategoryName
            }).ToList();
        }

        public async Task ClearCart(string sessionId, string userID, string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);
            await cartDAL.ClearCart(sessionId, userId);
        }


        public async Task RemoveCart(Guid productId, string userID, string sessionId, string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            await cartDAL.RemoveProduct(productId, userId, sessionId);
        }


        public async Task<CartModel> UpdateCartQuantity(Guid productId, string userID, string sessionId, string connectionString)
        {
            var cartDAL = new CartDAL(_dbContextFactory, connectionString);
            Guid? userId = null;
            if (!string.IsNullOrWhiteSpace(userID))
                userId = Guid.Parse(userID);

            var updatedProduct = await cartDAL.UpdateProductQuantity(productId, userId, sessionId);
            if (updatedProduct == null)
            {
                return null;
            }

          
            return new CartModel
            {
                productId = updatedProduct.productId,
                Title = updatedProduct.Title,
                Price = updatedProduct.Price,
                Kilo = updatedProduct.Kilo,
                CategoryName = updatedProduct.CategoryName,
                ImageUrl = updatedProduct.ImageUrl,
                Description = updatedProduct.Description,
                Quantity = updatedProduct.Quantity
              
            };
        }



    }
}

