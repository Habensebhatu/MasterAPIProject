using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace business_logic_layer
{
    public class CategoryBLL
    {
        private readonly IDbContextFactory _dbContextFactory;

        public CategoryBLL(IDbContextFactory dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<CategoryModel> AddCategory(CategoryModel category, string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            var categoryEntity = new Category()
            {
                CategoryId = category.categoryId,
                Name = category.Name,
                quantityProduct = category.quantityProduct
            };

            await categoryDAL.AddCategory(categoryEntity);
            return category;
        }

        public async Task<List<CategoryModel>> GetCategories(string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            var categories = await categoryDAL.GetCategories();
            return categories.Select(c => new CategoryModel
            {
                categoryId = c.CategoryId,
                Name = c.Name,
                quantityProduct = c.quantityProduct
            }).ToList();
        }

        public async Task<CategoryModel> GetCategoryById(Guid id, string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            var category = await categoryDAL.GetCategoryById(id);
            return category != null ? new CategoryModel { categoryId = category.CategoryId, Name = category.Name, quantityProduct = category.quantityProduct } : null;
        }

        public async Task<CategoryModel> GetCategoryByName(string name, string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            var category = await categoryDAL.GetCategoryByName(name);
            return category != null ? new CategoryModel { categoryId = category.CategoryId, Name = category.Name, quantityProduct = category.quantityProduct } : null;
        }

        public async Task<bool> RemoveCategory(Guid id, string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            return await categoryDAL.RemoveCategory(id);
        }

        public async Task<CategoryModel> UpdateCategory(Guid categoryId, CategoryModel category, string connectionString)
        {
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionString);
            var existingCategory = await categoryDAL.GetCategoryById(categoryId);
            if (existingCategory == null)
            {
                return null;
            }

            existingCategory.Name = category.Name;
            existingCategory.quantityProduct = category.quantityProduct;
            await categoryDAL.UpdateCategory(existingCategory);
            return new CategoryModel
            {
                categoryId = existingCategory.CategoryId,
                Name = existingCategory.Name,
                quantityProduct = existingCategory.quantityProduct
            };
        }
    }
}
