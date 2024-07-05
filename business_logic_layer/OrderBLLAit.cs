using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using System.Reflection.Emit;
using IdGen;

namespace business_logic_layer
{
    public class OrderBLLAit
    {
        private readonly IDbContextFactory _dbContextFactory;
        public OrderBLLAit(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<OrderModelAdd> AddOrder(OrderModelAdd order, string connectionString)
        {
            var orderDAL = new OrderDALAit(_dbContextFactory, connectionString);
            var generator = new IdGenerator(0);
            long uniqueId = generator.CreateId() % 100000000;

            OrderEntiyAit FormatOrder = new OrderEntiyAit()
            {
                OrderId = new Guid(),
                OrderDate = DateTime.Now,
                UserId = order.UserId,
                OrderNumber = uniqueId,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailEntityAit
                {
                    OrderDetailId = new Guid(),
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    AmountTotal = od.AmountTotal,
                    Contents = od.contents,
                    Price = od.Price


                }).ToList()
            };
            await orderDAL.AddOrder(FormatOrder);
            return order;
        }

        public async Task<List<OrderModelAit>> GetOrders(string connectionString)
        {
            var orderDAL = new OrderDALAit(_dbContextFactory, connectionString);
            List<OrderEntiyAit> ordersFromDb = await orderDAL.GetOrders();

            return ordersFromDb.Select(order => new OrderModelAit
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                UserId = order.UserId,
                recipientName = order.User.FirstName,
                OrderNumber = order.OrderNumber,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailModelAit
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Title = od.Product.Title,
                    Quantity = od.Quantity,
                    AmountTotal = od.AmountTotal,
                    ImageUrl = od.Product.ProductImages?.FirstOrDefault()?.ImageUrl,
                    Price = od.Price,
                    contents = od.Contents,
                    OrderId = od.OrderId

                }).ToList()
            }).ToList();
        }
        public async Task<GetOrderModelAit> GetOrderById(Guid id, string connectionString)
        {
            var orderDAL = new OrderDALAit(_dbContextFactory, connectionString);
            OrderEntiyAit orderFromDb = await orderDAL.GetOrderById(id);

            if (orderFromDb == null)
                return null;

            return new GetOrderModelAit
            {
                OrderId = orderFromDb.OrderId,
                UserId = orderFromDb.UserId,
                CustomerEmail = orderFromDb.User.Email, 
                recipientName = orderFromDb.User.FirstName,
                city = orderFromDb.User.Address.Residence,
                Street = orderFromDb.User.Address.Street,
                postalCode = orderFromDb.User.Address.ZipCode,
                phoneNumber = orderFromDb.User.Address.PhoneNumber,
                OrderDate = orderFromDb.OrderDate,
                OrderNumber = orderFromDb.OrderNumber,
                OrderDetails = orderFromDb.OrderDetails.Select(od => new OrderDetailModelAit
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Title = od.Product.Title,
                    Quantity = od.Quantity,
                    contents = od.Contents,
                    AmountTotal = od.AmountTotal,
                    ImageUrl = od.Product.ProductImages?.FirstOrDefault()?.ImageUrl,
                    Price = od.Price
                }).ToList()
            };
        }
    }
}

