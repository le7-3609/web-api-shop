using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace Tests.IntegretionTests
{
    public class ProductRepositoryIntegrationTests : IDisposable
    {
        private readonly MyShopContext _context;
        private readonly ProductRepository _repository;

        public ProductRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _context.ChangeTracker.Clear();

            // Clear tables in correct order to avoid FK conflicts
            _context.Products.RemoveRange(_context.Products);
            _context.SubCategories.RemoveRange(_context.SubCategories);
            _context.MainCategories.RemoveRange(_context.MainCategories);
            _context.SaveChanges();

            _repository = new ProductRepository(_context);
        }

        [Fact]
        public async Task AddProductAsync_SavingToDb_ShouldWorkWithRequiredFields()
        {
            // Arrange
            var product = new Product
            {
                ProductName = "Kosher Phone",
                ProductPrompt = "Must have this field", 
                SubCategoryId = 5
            };

            // Act
            var result = await _repository.AddProductAsync(product);

            // Assert
            Assert.NotEqual(0, result.ProductId); 
            var exists = await _context.Products.AnyAsync(p => p.ProductName == "Kosher Phone");
            Assert.True(exists);
        }

        [Fact]
        public async Task GetProductsBySubCategoryIdAsync_FiltersCorrectSubCategory()
        {
            // Arrange
            var subCategory = new SubCategory
            {
                SubCategoryId = 1,
                SubCategoryName = "Electronics",
                SubCategoryPrompt = "Required description content" 
            };

            await _context.SubCategories.AddAsync(subCategory);

            _context.Products.AddRange(
                new Product { ProductName = "P1", SubCategoryId = 1, ProductPrompt = "A" },
                new Product { ProductName = "P2", SubCategoryId = 1, ProductPrompt = "B" }
            );

            await _context.SaveChangesAsync();

            // Act
            var results = await _repository.GetProductsBySubCategoryIdAsync(1);

            // Assert
            Assert.Equal(2, results.Count());
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldChangeDataInDb()
        {
            // Arrange
            var product = new Product { ProductId = 10, ProductName = "Old Name", ProductPrompt = "A" };
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            // Act
            product.ProductName = "New Name";
            await _repository.UpdateProductAsync(10, product);

            // Assert
            var updated = await _context.Products.FindAsync(10);
            Assert.Equal("New Name", updated.ProductName);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenProductDoesNotExist_ReturnsFalse()
        {
            // Act
            var result = await _repository.DeleteProductAsync(999);

            // Assert
            Assert.False(result);
        }
        public void Dispose()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}