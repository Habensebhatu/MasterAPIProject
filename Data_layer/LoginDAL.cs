using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class LoginDAL
    {
        private readonly DbContext _context;

        public LoginDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<LoginEnitiyModel> GetUserByEmail(string username)
        {
            return await _context.Set<LoginEnitiyModel>().FirstOrDefaultAsync(u => u.useName == username);
        }
    }
}
