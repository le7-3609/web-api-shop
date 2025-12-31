using Entities;
using Microsoft.EntityFrameworkCore;
using Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Tests.IntegretionTests
{
    public class OrderRepositoryIntegrationTests : IClassFixture<DatabaseFixture>
    {
        private readonly MyShopContext _context;
        private readonly OrderRepository _repository;

        public OrderRepositoryIntegrationTests(DatabaseFixture fixture)
        {
            _context = fixture.Context;
            _context.ChangeTracker.Clear();

            // Comprehensive cleaning in correct order to prevent FK issues
            _context.OrderItems.RemoveRange(_context.OrderItems);
            _context.Reviews.RemoveRange(_context.Reviews);
            _context.Orders.RemoveRange(_context.Orders);
            _context.Products.RemoveRange(_context.Products);
            _context.Platforms.RemoveRange(_context.Platforms);
            _context.BasicSites.RemoveRange(_context.BasicSites);
            _context.SiteTypes.RemoveRange(_context.SiteTypes);
            _context.SubCategories.RemoveRange(_context.SubCategories);
            _context.MainCategories.RemoveRange(_context.MainCategories);
            _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

            _repository = new OrderRepository(_context);
        }

        private async Task<(int userId, int siteId, int productId, int platformId)> SeedBaseDataAsync()
        {
            // Seed Site Structure
            var siteType = new SiteType
            {
                SiteTypeName = "Default",
                SiteTypeNamePrompt = "Name Prompt",
                SiteTypeDescriptionPrompt = "Description Prompt"
            };
            var mainCat = new MainCategory { MainCategoryName = "General", MainCategoryPrompt = "P" };
            await _context.SiteTypes.AddAsync(siteType);
            await _context.MainCategories.AddAsync(mainCat);
            await _context.SaveChangesAsync();

            var subCat = new SubCategory { SubCategoryName = "Hardware", SubCategoryPrompt = "P", MainCategoryId = mainCat.MainCategoryId };
            await _context.SubCategories.AddAsync(subCat);

            // Seed User and Site
            var user = new User { Email = $"{Guid.NewGuid()}@test.com", Password = "1", FirstName = "A", LastName = "B", Phone = "0" };
            var site = new BasicSite { SiteName = "Store", SiteTypeId = siteType.SiteTypeId };
            await _context.Users.AddAsync(user);
            await _context.BasicSites.AddAsync(site);
            await _context.SaveChangesAsync();

            // Seed Product and Platform
            var platform = new Platform { PlatformName = "Web" };
            var product = new Product { ProductName = "Item", SubCategoryId = subCat.SubCategoryId, ProductPrompt = "P" };
            await _context.Platforms.AddAsync(platform);
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return (user.UserId, site.BasicSiteId, product.ProductId, platform.PlatformId);
        }

        #region Happy Paths

        [Fact]
        public async Task AddOrderAsync_ShouldSaveOrderWithValidItems()
        {
            // Arrange
            var data = await SeedBaseDataAsync();
            var order = new Order
            {
                UserId = data.userId,
                BasicSiteId = data.siteId,
                OrderDate = DateOnly.FromDateTime(DateTime.Now),
                OrderSum = 1000,
                OrderItems = new List<OrderItem> { new OrderItem { ProductId = data.productId, PlatformId = data.platformId, UserDescription = "D" } }
            };

            // Act
            var result = await _repository.AddOrderAsync(order);

            // Assert
            Assert.NotEqual(0, result.OrderId);
        }

        [Fact]
        public async Task AddReviewAsync_ShouldSaveAndLinkToOrder()
        {
            // Arrange
            var data = await SeedBaseDataAsync();
            var order = new Order { UserId = data.userId, BasicSiteId = data.siteId, OrderDate = DateOnly.FromDateTime(DateTime.Now), OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            var review = new Review { OrderId = order.OrderId, Score = 5, Note = "Good" };

            // Act
            var result = await _repository.AddReviewAsync(review);

            // Assert
            Assert.NotNull(await _context.Reviews.FindAsync(result.ReviewId));
        }

        [Fact]
        public async Task GetOrderItemsAsync_ReturnsItemsForSpecificOrder()
        {
            // Arrange
            var data = await SeedBaseDataAsync();
            var order = new Order { UserId = data.userId, BasicSiteId = data.siteId, OrderDate = DateOnly.FromDateTime(DateTime.Now), OrderSum = 200 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
            await _context.OrderItems.AddAsync(new OrderItem { OrderId = order.OrderId, ProductId = data.productId, PlatformId = data.platformId, UserDescription = "I" });
            await _context.SaveChangesAsync();

            // Act
            var items = await _repository.GetOrderItemsAsync(order.OrderId);

            // Assert
            Assert.NotEmpty(items);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(9999);
            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task GetReviewByOrderIdAsync_NoReviewExists_ReturnsNull()
        {
            // Arrange
            var data = await SeedBaseDataAsync();
            var order = new Order { UserId = data.userId, BasicSiteId = data.siteId, OrderDate = DateOnly.FromDateTime(DateTime.Now), OrderSum = 100 };
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetReviewByOrderIdAsync(order.OrderId);

            // Assert
            Assert.Null(result);
        }

        #endregion
    }
}