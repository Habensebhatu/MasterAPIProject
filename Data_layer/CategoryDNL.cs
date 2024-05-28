using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class CategoryDNL
    {
        private readonly DbContext _context;

        public CategoryDNL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<Category> AddCategory(Category category)
        {
            _context.Set<Category>().Add(category);
            await _context.SaveChangesAsync();
            return category;
        }

        public async Task<List<Category>> GetCategories()
        {
            return await _context.Set<Category>().ToListAsync();
        }

        public async Task<Category> GetCategoryById(Guid categoryId)
        {
            return await _context.Set<Category>().FirstOrDefaultAsync(c => c.CategoryId == categoryId);
        }

        public async Task<Category> GetCategoryByName(string name)
        {
            return await _context.Set<Category>().FirstOrDefaultAsync(c => c.Name == name);
        }

        public async Task<bool> RemoveCategory(Guid categoryId)
        {
            var category = await _context.Set<Category>().FindAsync(categoryId);
            if (category != null)
            {
                _context.Set<Category>().Remove(category);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task UpdateCategory(Category category)
        {
            _context.Set<Category>().Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
