using System;
using Data_layer.Context;
using Data_layer.Data;
using Microsoft.EntityFrameworkCore;

namespace Data_layer
{
    public class ProductDAL
    {
        

        private readonly DbContext _context;

        public ProductDAL(IDbContextFactory dbContextFactory, string connectionStringName)
        {
            _context = dbContextFactory.CreateDbContext(connectionStringName);
        }

        public async Task<Product> AddProduct(Product product, List<string> imageUrls)
        {
            _context.Set<Product>().Add(product);
            await _context.SaveChangesAsync();
            Console.WriteLine($"imageUrls: {imageUrls}");

            for (int i = 0; i < imageUrls.Count; i++)
            {
                var productImage = new ProductImageEnityModel
                {
                    ImageUrl = imageUrls[i],
                    ProductId = product.ProductId,
                    Index = i
                };
                Console.WriteLine($"Setting index for {imageUrls[i]}: {i}");
                _context.Set<ProductImageEnityModel>().Add(productImage);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("ex.Message", ex.Message);
                if (ex.InnerException != null)
                {
                    Console.WriteLine("ex.InnerException.Message", ex.InnerException.Message);
                }
            }

            return product;
        }



        public async Task<List<Product>> GetProductsByName(string category, int pageNumber, int pageSize)
        {
            return await _context.Set<Product>().Include(p => p.Category)
                                         .Include(p => p.ProductImages)
                                         .Where(p => p.Category.Name == category)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
        }

        public async Task<List<Product>> GetProducts()
        {
            return await _context.Set<Product>()
                .Include(p => p.Category)
                .Include(p => p.ProductImages)
                .OrderBy(p => p.ProductImages.Min(img => img.Index))
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsPageNumber(int pageNumber, int pageSize)
        {
            return await _context.Set<Product>().Include(p => p.Category)
                                         .Include(p => p.ProductImages)
                                         .Skip((pageNumber - 1) * pageSize)
                                         .Take(pageSize)
                                         .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByNameAndPrice(string category, decimal minPrice, decimal? maxPrice, int pageNumber, int pageSize)
        {
            var query = _context.Set<Product>().Include(p => p.Category)
                                        .Include(p => p.ProductImages)
                                        .Where(p => p.Category.Name == category && p.PiecePrice >= minPrice);

            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.PiecePrice <= maxPrice.Value);
            }

            return await query.Skip((pageNumber - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();
        }

        public async Task<Product> GetProductById(Guid id)
        {
            return await _context.Set<Product>().Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.ProductId == id);
        }

        

        public async Task<Product> GetProductsByProductName(string product)
        {
            return await _context.Set<Product>().Include(p => p.Category).Include(p => p.ProductImages).FirstOrDefaultAsync(p => p.Title.Contains(product));
        }

        public async Task<List<Product>> fillterPrice(decimal min, decimal max)
        {
            return await _context.Set<Product>().Include(p => p.Category).Where(p => p.PiecePrice >= min && p.PiecePrice <= max).ToListAsync();
        }


        public async Task<List<Product>> SearchProductsByProductName(string product)
        {
            return await _context.Set<Product>().Include(p => p.Category).Include(p => p.ProductImages).Where(p => p.Title.StartsWith(product)).ToListAsync();

        }

        public async Task RemoveProduct(Guid id)
        {
            var product = await GetProductById(id);
            if (product != null)
            {
                _context.Set<Product>().Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Product> UpdateProduct(Product product, List<ExistingImageMode> imageModels)
        {
            var existingProduct = _context.Set<Product>().Find(product.ProductId);
            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(product);
                var existingImages = _context.Set<ProductImageEnityModel>().Where(pi => pi.ProductId == product.ProductId).ToList();


                foreach (var existingImage in existingImages)
                {
                    if (!imageModels.Any(imgModel => imgModel.file == existingImage.ImageUrl))
                    {
                        _context.Set<ProductImageEnityModel>().Remove(existingImage);
                    }
                }

                foreach (var imageModel in imageModels)
                {
                    if (!existingImages.Any(ei => ei.ImageUrl == imageModel.file))
                    {
                        var productImage = new ProductImageEnityModel
                        {
                            ImageUrl = imageModel.file,
                            ProductId = product.ProductId,
                            Index = imageModel.index
                        };
                        _context.Set<ProductImageEnityModel>().Add(productImage);
                    }
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                Console.WriteLine("Product not found!");
            }

            return product;
        }

        public async Task<List<Product>> GetPopularProducts()
        {
            return await _context.Set<Product>().Include(p => p.Category).Include(p => p.ProductImages).Where(p => p.IsPopular).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategory(string category)
        {
            return await _context.Set<Product>()
                                 .Include(p => p.Category)
                                 .Include(p => p.ProductImages)
                                 .Where(p => p.Category.Name == category)
                                 .ToListAsync();
        }

        public async Task UpdateProductStock(Guid productId, int quantitySold, decimal price)
        {
            var product = _context.Set<Product>().FirstOrDefault(item => item.ProductId == productId);

            if (product != null)
            {
                if (price == product.PiecePrice)
                {
                    product.InstokeOfPiece -= quantitySold;
                    if (product.InstokeOfPiece < 0)
                    {
                        throw new InvalidOperationException("Cannot reduce piece stock below zero.");
                    }
                }
                else if (price == product.CratePrice)
                {
                    product.InstokeOfCrate -= quantitySold;
                    if (product.InstokeOfCrate < 0)
                    {
                        throw new InvalidOperationException("Cannot reduce crate stock below zero.");
                    }
                }
                else
                {
                    throw new ArgumentException("Price does not match any product price.");
                }

                await _context.SaveChangesAsync();
            }
            else
            {
                throw new KeyNotFoundException("Product not found.");
            }
        }


    }
}

