using Data_layer.Context.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class UserRegistrationDAL
    {
        private readonly DbContext _context;
      
        public UserRegistrationDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<UserRegistrationEntityModel> AddUser(UserRegistrationEntityModel user)
        {
            _context.Set<UserRegistrationEntityModel>().Add(user);
          
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<List<UserRegistrationEntityModel>> GetAllUsers()
        {
            return await _context.Set<UserRegistrationEntityModel>().ToListAsync();
        }


        public async Task<UserRegistrationEntityModel> GetUserByEmail(string email)
        {
            return await _context.Set<UserRegistrationEntityModel>().FirstOrDefaultAsync(u => u.Email == email);
        }


        public async Task<UserRegistrationEntityModel> GetUserById(Guid userId)
        {
            return await _context.Set<UserRegistrationEntityModel>()
                                 .Include(u => u.Address)
                                 .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task Removeuser(Guid id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _context.Set<UserRegistrationEntityModel>().Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<UserRegistrationEntityModel> Update(Guid userId)
        {
            var user = await GetUserById(userId);
            if (user != null)
            {
                user.IsApproved = true;
                _context.Set<UserRegistrationEntityModel>().Update(user);
                await _context.SaveChangesAsync();
                return user;
            }
            return null;
        }

    }
}

