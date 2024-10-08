﻿using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;


namespace business_logic_layer
{
    public class customerBLL
    {
    
        private readonly IDbContextFactory _dbContextFactory;
        public customerBLL(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }
        public async Task<CustomerModel> AddCustomer(CustomerModel customer, string connectionStringName)
        {
            var customerDAL = new CustomerDAL(_dbContextFactory, connectionStringName);
            CustomerEntityModel FormatOrder = new CustomerEntityModel()
            {
                CustomerId = customer.CustomerId,
                recipientName = customer.recipientName,
                CustomerEmail = customer.CustomerEmail,
                city = customer.city,
                line1 = customer.line1,
                phoneNumber = customer.phoneNumber,
                postalCode = customer.postalCode

            };
            await customerDAL.AddCustomer(FormatOrder);
            return customer;

        }

        public async Task<CustomerModel> GetCustomerByEmail(string customerEmail, string connectionStringName)
        {
            var customerDAL = new CustomerDAL(_dbContextFactory, connectionStringName);
            var customer = await customerDAL.GetCustomerBYEmail(customerEmail);
            if (customer == null)
            {
                return null;
            }
            return new CustomerModel
            {
                CustomerId = customer.CustomerId,
                recipientName = customer.recipientName,
                city = customer.city,
                phoneNumber = customer.phoneNumber,
                CustomerEmail = customer.CustomerEmail,
                postalCode = customer.postalCode,
                line1 = customer.line1

            };
        }

        public async Task<List<CustomerModel>> GetCustomers(string connectionStringName)
        {
            var customerDAL = new CustomerDAL(_dbContextFactory, connectionStringName);
            List<CustomerEntityModel> customers = await customerDAL.GetCustomers();
            if (customers == null)
            {
                return null;
            }

            List<CustomerModel> customerModel = customers.Select(c => new CustomerModel
            {
                CustomerId = c.CustomerId,
                CustomerEmail = c.CustomerEmail,
                city = c.city,
                line1 = c.line1,
                phoneNumber = c.phoneNumber,
                postalCode = c.postalCode,
                recipientName =c.recipientName
            }).ToList();

            return customerModel;

        }
    }
}

