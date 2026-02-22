using Entities;
using Repositories;
using Xunit;

namespace Tests.IntegrationTests
{
    [Collection("Database collection")]
    public class ProductRepositoryIntegrationTests
    {
        private readonly DatabaseFixture _fixture;
        private readonly MyShopContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _fixture = fixture;
            _context = fixture.Context;
            _repository = new ProductRepository(_context);
            _fixture.ClearDatabase();
        }

        private void SeedMainAndSubCategory()
        {
            var mainCat = new MainCategory { MainCategoryId = 1, MainCategoryName = "Main", MainCategoryPrompt = "P" };
            _context.MainCategories.Add(mainCat);
            var subCat = new SubCategory { SubCategoryId = 1, SubCategoryName = "Sub", SubCategoryPrompt = "P", MainCategoryId = 1 };
            _context.SubCategories.Add(subCat);
            _context.SaveChanges();
        }

        #region Happy Paths

        [Fact]
        public async Task AddProductAsync_ValidProduct_ReturnsProduct()
        {
            SeedMainAndSubCategory();
            var product = new Product { ProductName = "Laptop", ProductPrompt = "Required", SubCategoryId = 1 };

            var result = await _repository.AddProductAsync(product);

            Assert.True(result.ProductId > 0);
            Assert.Equal("Laptop", result.ProductName);
        }

        [Fact]
        public async Task GetProductByIdAsync_WithValidId_ReturnsProduct()
        {
            SeedMainAndSubCategory();
            var product = new Product { ProductId = 5, ProductName = "Phone", ProductPrompt = "P", SubCategoryId = 1 };
            _context.Products.Add(product);
            _context.SaveChanges();

            var result = await _repository.GetProductByIdAsync(5);

            Assert.NotNull(result);
            Assert.Equal("Phone", result.ProductName);
        }

        [Fact]
        public async Task GetProductsAsync_FilterBySubCategory_ReturnsFiltered()
        {
            SeedMainAndSubCategory();
            _context.Products.AddRange(
                new Product { ProductName = "P1", ProductPrompt = "P", SubCategoryId = 1 },
                new Product { ProductName = "P2", ProductPrompt = "P", SubCategoryId = 1 }
            );
            _context.SaveChanges();

            var (products, total) = await _repository.GetProductsAsync(10, 0, null, new int?[] { 1 });

            Assert.Equal(2, products.Count());
            Assert.Equal(2, total);
        }

        [Fact]
        public async Task UpdateProductAsync_WithValidProduct_Updates()
        {
            SeedMainAndSubCategory();
            var product = new Product { ProductId = 10, ProductName = "Old", ProductPrompt = "P", SubCategoryId = 1 };
            _context.Products.Add(product);
            _context.SaveChanges();

            product.ProductName = "New";
            await _repository.UpdateProductAsync(10, product);

            var updated = _context.Products.Find(10L);
            Assert.NotNull(updated);
            Assert.Equal("New", updated.ProductName);
        }

        [Fact]
        public async Task GetProductsBySubCategoryIdAsync_ReturnsCorrectProducts()
        {
            SeedMainAndSubCategory();
            _context.Products.AddRange(
                new Product { ProductName = "P1", ProductPrompt = "P", SubCategoryId = 1 },
                new Product { ProductName = "P2", ProductPrompt = "P", SubCategoryId = 1 }
            );
            _context.SaveChanges();

            var result = await _repository.GetProductsBySubCategoryIdAsync(1);

            Assert.Equal(2, result.Count());
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetProductByIdAsync_NonExistentId_ReturnsNull()
        {
            var result = await _repository.GetProductByIdAsync(9999);
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteProductAsync_NonExistentProduct_ReturnsFalse()
        {
            var result = await _repository.DeleteProductAsync(9999);
            Assert.False(result);
        }

        [Fact]
        public async Task GetProductsAsync_EmptyDatabase_ReturnsEmpty()
        {
            var (products, total) = await _repository.GetProductsAsync(10, 0, null, Array.Empty<int?>());

            Assert.Empty(products);
            Assert.Equal(0, total);
        }

        [Fact]
        public async Task GetProductsBySubCategoryIdAsync_NonExistentSubCategory_ReturnsEmpty()
        {
            var result = await _repository.GetProductsBySubCategoryIdAsync(9999);

            Assert.Empty(result);
        }

        #endregion
    }
}