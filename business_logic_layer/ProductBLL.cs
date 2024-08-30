using System;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using static System.Net.Mime.MediaTypeNames;


namespace business_logic_layer
{
    public class ProductBLL
    {
        private readonly CategoryDNL _CategoryDAL;
        private readonly ProductDAL _ProductDAL;
        private readonly IDbContextFactory _dbContextFactory;
        private readonly string azureConnectionString;

        public ProductBLL(IDbContextFactory dbContextFactory, IConfiguration configuration)
        {

            _dbContextFactory = dbContextFactory;
            azureConnectionString = configuration.GetConnectionString("AzureblobConnectionString");

        }

        public async Task<productModel> AddProduct(productModel product, string connectionStringName)
        {

            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);

            List<string> imageUrls = new List<string>();
            foreach (var image in product.NewImages)
            {
                imageUrls.Add(await UploadImageToAzure(image));
            }

            Product productFormat = new Product()
            {
                ProductId = product.ProductId,
                Title = product.Title,
                PiecePrice = product.PiecePrice,
                Kilo = product.Kilo,
                InstokeOfPiece = product.InstokeOfPiece,
                CratePrice = product.CratePrice,
                CrateQuantity = product.CrateQuantity,
                InstokeOfCrate = product.InstokeOfCrate,
                Description = product.Description,
                CategoryId = product.CategoryId,
                IsPopular = product.IsPopular,


            };
            Console.WriteLine($"business_logic_layer imageUrls count: {imageUrls.Count}");


            await ProductDAL.AddProduct(productFormat, imageUrls);

            return product;
        }


        private async Task<string> UploadImageToAzure(IFormFile image)
        {
            var blobServiceClient = new BlobServiceClient(azureConnectionString);
            var blobContainerClient = blobServiceClient.GetBlobContainerClient("blolcontainerws");
            string url = string.Empty;

            if (image != null && image.Length > 0)
            {
                var blobClient = blobContainerClient.GetBlobClient(image.FileName);

                using (var stream = image.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = image.ContentType });
                }

                url = blobClient.Uri.AbsoluteUri;
            }

            return url;
        }

        public async Task<List<productModelS>> GetProductsByName(string category, int pageNumber, int pageSize, string connectionStringName)
        {

            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductsByName(category, pageNumber, pageSize);
            if (products == null)
            {
                return null;
            }
            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();

        }

        public async Task<List<productModelS>> GetProducts(string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProducts();
            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name,
                IsPopular = p.IsPopular,
                ImageUrls = p.ProductImages
                    .OrderBy(pi => pi.Index)  // <-- Order the ProductImages by their Index here
                    .Select(pi => new ImageUpdateModel
                    {
                        Index = pi.Index,
                        File = pi.ImageUrl // Assuming ImageUrl is the file path or URL you want
                    }).ToList()
            }).ToList();
        }


        public async Task<List<productModelS>> GetProductsPageNumber(int pageNumber, int pageSize, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductsPageNumber(pageNumber, pageSize);

            return products.Select(p => new productModelS
            {

                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }

        public async Task<List<productModelS>> GetProductsByNameAndPrice(string category, decimal minPrice, decimal? maxPrice, int pageNumber, int pageSize, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductsByNameAndPrice(category, minPrice, maxPrice, pageNumber, pageSize);

            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }

        public async Task<productModelS> GetProductById(Guid id, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductById(id);
            if (products == null)
            {
                return null;
            }
            return new productModelS
            {
                ProductId = products.ProductId,
                Title = products.Title,
                PiecePrice = products.PiecePrice,
                Kilo = products.Kilo,
                InstokeOfPiece = products.InstokeOfPiece,
                CratePrice = products.CratePrice,
                CrateQuantity = products.CrateQuantity,
                InstokeOfCrate = products.InstokeOfCrate,

                Description = products.Description,
                ImageUrls = products.ProductImages
                                    .OrderBy(pi => pi.Index)  // <-- Order the ProductImages by their Index here
                                    .Select(pi => new ImageUpdateModel
                                    {
                                        Index = pi.Index,
                                        File = pi.ImageUrl // Assuming ImageUrl is the file path or URL you want
                                    }).ToList(),
                CategoryId = products.CategoryId,
                CategoryName = products.Category.Name
            };
        }

        public async Task<StripeImage> GetProductsByProductName(string product, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductsByProductName(product);



            return new StripeImage
            {
                Title = products.Title,
                Price = products.PiecePrice,
                Kilo = products.Kilo,
                Description = products.Description,
                CategoryId = products.CategoryId,
                productId = products.ProductId,
                ImageUrls = products.ProductImages?.FirstOrDefault()?.ImageUrl

            };

        }

        public async Task<List<StripeImage>> fillterPrice(decimal min, decimal max, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.fillterPrice(min, max);

            return products.Select(p => new StripeImage
            {
                productId = p.ProductId,
                Title = p.Title,
                Price = p.PiecePrice,
                Kilo = p.Kilo,
                Description = p.Description,
                ImageUrls = p.ProductImages?.FirstOrDefault()?.ImageUrl,
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();
        }


        public async Task<List<productModelS>> SearchProductsByProductName(string product, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.SearchProductsByProductName(product);

            if (products == null)
            {
                return null;
            }
            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();

        }

        public async Task RemoveProduct(Guid id, string connectionStringName)
        {

            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            await ProductDAL.RemoveProduct(id);
        }


        public async Task<productModel> UpdateProduct(productModel product, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var categoryDAL = new CategoryDNL(_dbContextFactory, connectionStringName);
            var categoryBYName = await categoryDAL.GetCategoryByName(product.CategoryName);
            List<ExistingImageUrlModel> allImageUrls = new List<ExistingImageUrlModel>(product.ExistingImageUrls);

            for (int i = 0; i < product.NewImages.Count; i++)
            {
                var image = product.NewImages[i];
                var index = product.NewImageIndices[i]; // Fetch the index

                var imageUrl = await UploadImageToAzure(image);
                allImageUrls.Add(new ExistingImageUrlModel
                {
                    file = imageUrl,
                    index = index
                });
            }

            Product productFormat = new Product()
            {
                ProductId = product.ProductId,
                Title = product.Title,
                PiecePrice = product.PiecePrice,
                Kilo = product.Kilo,
                InstokeOfPiece = product.InstokeOfPiece,
                CratePrice = product.CratePrice,
                CrateQuantity = product.CrateQuantity,
                InstokeOfCrate = product.InstokeOfCrate,
                Description = product.Description,
                CategoryId = categoryBYName.CategoryId,
                IsPopular = product.IsPopular
            };
            var formattedImages = allImageUrls.Select(i => new ExistingImageMode()
            {
                file = i.file,
                index = i.index
            }).ToList();

            await ProductDAL.UpdateProduct(productFormat, formattedImages);

            return product;
        }

        public async Task<List<productModelS>> GetPopularProducts(string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetPopularProducts();
            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();

        }

        public async Task<List<productModelS>> GetProductsByCategory(string category, string connectionStringName)
        {
            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            var products = await ProductDAL.GetProductsByCategory(category);
            return products.Select(p => new productModelS
            {
                ProductId = p.ProductId,
                Title = p.Title,
                PiecePrice = p.PiecePrice,
                Kilo = p.Kilo,
                InstokeOfPiece = p.InstokeOfPiece,
                CratePrice = p.CratePrice,
                CrateQuantity = p.CrateQuantity,
                InstokeOfCrate = p.InstokeOfCrate,
                Description = p.Description,
                ImageUrls = p.ProductImages
                            .OrderBy(pi => pi.Index)
                            .Select(pi => new ImageUpdateModel
                            {
                                Index = pi.Index,
                                File = pi.ImageUrl
                            }).ToList(),
                CategoryId = p.CategoryId,
                CategoryName = p.Category.Name
            }).ToList();

        }

        public async Task UpdateProductStock(Guid productId, int quantity, decimal price, string connectionStringName)
        {

            var ProductDAL = new ProductDAL(_dbContextFactory, connectionStringName);
            await ProductDAL.UpdateProductStock(productId, quantity, price);
        }

    }

}
