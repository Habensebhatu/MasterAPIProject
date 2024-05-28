using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class CustomerDAL
    {
      
        private readonly DbContext _context;
        public CustomerDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }
        public async Task<CustomerEntityModel> AddCustomer(CustomerEntityModel customer)

        {
            _context.Set<CustomerEntityModel>().Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }

        public async Task<List<CustomerEntityModel>> GetCustomers()
        {
            return await _context.Set<CustomerEntityModel>().ToListAsync();
        }

        public async Task<CustomerEntityModel> GetCustomerBYEmail(string customerEmail)
        {
            CustomerEntityModel customer = await _context.Set<CustomerEntityModel>().FirstOrDefaultAsync(x => x.CustomerEmail == customerEmail);
            if (customer == null)
            {
                return null;
            }
            return customer;
        }
    }

}

