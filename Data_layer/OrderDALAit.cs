using System;
using Data_layer.Context;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
	public class OrderDALAit
	{
        private readonly DbContext _context;
        public OrderDALAit(IDbContextFactory dbContextFactory, string connectionStringName)
		{
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<OrderEntiyAit> AddOrder(OrderEntiyAit order)

        {
            _context.Set<OrderEntiyAit>().Add(order);
            await _context.SaveChangesAsync();
            return order;
        }

        public async Task<List<OrderEntiyAit>> GetOrders()
        {
            return await _context.Set<OrderEntiyAit>().Include(o => o.User)
                           .Include(o => o.OrderDetails)
                               .ThenInclude(od => od.Product)
                           .ToListAsync();
        }

        public async Task<OrderEntiyAit> GetOrderById(Guid id)
        {
            return await _context.Set<OrderEntiyAit>()
                       .Include(o => o.User)
                       .Include(o => o.OrderDetails)
                           .ThenInclude(od => od.Product).ThenInclude(od => od.ProductImages)
                       .FirstOrDefaultAsync(o => o.OrderId == id);
        }
    }
}

