using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Entities;
using Moq;
using Repositories;
using Services;
using Xunit;

namespace Tests.UnitTests
{
    public class ProductServiceUnitTests
    {
        private static Mock<IProductCacheService> BuildNoOpCache()
        {
            var mockCache = new Mock<IProductCacheService>();
            mockCache.Setup(c => c.GetProductAsync(It.IsAny<long>())).ReturnsAsync((ProductDTO?)null);
            mockCache.Setup(c => c.GetProductListAsync(It.IsAny<string>())).ReturnsAsync(((IEnumerable<ProductDTO>?)null, 0));
            mockCache.Setup(c => c.BuildListCacheKeyAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int?[]>())).ReturnsAsync("test-key");
            mockCache.Setup(c => c.SetProductAsync(It.IsAny<long>(), It.IsAny<ProductDTO>())).Returns(Task.CompletedTask);
            mockCache.Setup(c => c.SetProductListAsync(It.IsAny<string>(), It.IsAny<IEnumerable<ProductDTO>>(), It.IsAny<int>())).Returns(Task.CompletedTask);
            mockCache.Setup(c => c.InvalidateProductAsync(It.IsAny<long>())).Returns(Task.CompletedTask);
            mockCache.Setup(c => c.InvalidateProductListsAsync()).Returns(Task.CompletedTask);
            return mockCache;
        }

        [Fact]
        public async Task AddProductAsync_Throws_OnEmptyName()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            await Assert.ThrowsAsync<ArgumentException>(() => svc.AddProductAsync(new AdminProductDTO(null, 1, "", 10, "p")));
        }

        [Fact]
        public async Task AddProductAsync_SavesAndReturnsDto()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();

            var dto = new AdminProductDTO(null, 1, "Widget", 10, "p");
            var mapped = new Product { ProductName = "Widget" };
            mockMapper.Setup(m => m.Map<Product>(dto)).Returns(mapped);

            var saved = new Product { ProductId = 88, ProductName = "Widget", ProductPrompt = "p" };
            mockRepo.Setup(r => r.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(saved);
            var outDto = new ProductDTO(88, 1, "Widget", "Sub", 10, "p");
            mockMapper.Setup(m => m.Map<ProductDTO>(saved)).Returns(outDto);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            var res = await svc.AddProductAsync(dto);

            mockRepo.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Once);
            Assert.Equal(outDto.ProductId, res.ProductId);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenNotFound_ReturnsFalse()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetProductByIdAsync(99)).ReturnsAsync((Product)null!);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            var result = await svc.DeleteProductAsync(99);

            Assert.False(result);
            mockRepo.Verify(r => r.DeleteProductAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProductAsync_WithOrders_ThrowsInvalidOperationException()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, ProductName = "P" });
            mockRepo.Setup(r => r.HasOrderItemsByProductIdAsync(1)).ReturnsAsync(true);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            var ex = await Assert.ThrowsAsync<InvalidOperationException>(() => svc.DeleteProductAsync(1));
            Assert.Contains("orders", ex.Message, StringComparison.OrdinalIgnoreCase);
            mockRepo.Verify(r => r.DeleteProductAsync(It.IsAny<int>()), Times.Never);
            mockRepo.Verify(r => r.RemoveCartItemsByProductIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task DeleteProductAsync_WithCartItems_RemovesCartItemsThenDeletes()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, ProductName = "P" });
            mockRepo.Setup(r => r.HasOrderItemsByProductIdAsync(1)).ReturnsAsync(false);
            mockRepo.Setup(r => r.RemoveCartItemsByProductIdAsync(1)).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.DeleteProductAsync(1)).ReturnsAsync(true);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            var result = await svc.DeleteProductAsync(1);

            Assert.True(result);
            mockRepo.Verify(r => r.RemoveCartItemsByProductIdAsync(1), Times.Once);
            mockRepo.Verify(r => r.DeleteProductAsync(1), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_NoOrdersNoCartItems_DeletesSuccessfully()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, ProductName = "P" });
            mockRepo.Setup(r => r.HasOrderItemsByProductIdAsync(1)).ReturnsAsync(false);
            mockRepo.Setup(r => r.RemoveCartItemsByProductIdAsync(1)).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.DeleteProductAsync(1)).ReturnsAsync(true);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, BuildNoOpCache().Object);

            var result = await svc.DeleteProductAsync(1);

            Assert.True(result);
        }

        #region Cache Interaction

        [Fact]
        public async Task GetProductByIdAsync_CacheHit_DoesNotCallRepository()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();
            var cached = new ProductDTO(1, 2, "Cached", "Sub", 9.99, "prompt");
            mockCache.Setup(c => c.GetProductAsync(1)).ReturnsAsync(cached);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            var result = await svc.GetProductByIdAsync(1);

            Assert.Equal(cached, result);
            mockRepo.Verify(r => r.GetProductByIdAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetProductByIdAsync_CacheMiss_PopulatesCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            var entity = new Product { ProductId = 1, ProductName = "DB" };
            var dto = new ProductDTO(1, 2, "DB", "Sub", 5.0, "p");
            mockRepo.Setup(r => r.GetProductByIdAsync(1)).ReturnsAsync(entity);
            mockMapper.Setup(m => m.Map<ProductDTO>(entity)).Returns(dto);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            var result = await svc.GetProductByIdAsync(1);

            Assert.Equal(dto, result);
            mockCache.Verify(c => c.SetProductAsync(1, dto), Times.Once);
        }

        [Fact]
        public async Task GetProductsAsync_CacheHit_DoesNotCallRepository()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            var cachedList = new List<ProductDTO> { new(1, 2, "Cached", "Sub", 9.0, "p") };
            mockCache.Setup(c => c.GetProductListAsync("test-key")).ReturnsAsync((cachedList, 1));

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            var (items, total) = await svc.GetProductsAsync(10, 0, null, []);

            Assert.Equal(1, total);
            Assert.Single(items);
            mockRepo.Verify(r => r.GetProductsAsync(
                It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<int?[]>()), Times.Never);
        }

        [Fact]
        public async Task GetProductsAsync_CacheMiss_PopulatesCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            var entities = new List<Product> { new() { ProductId = 1, ProductName = "DB" } };
            var dtos = new List<ProductDTO> { new(1, 2, "DB", "Sub", 5.0, "p") };
            mockRepo.Setup(r => r.GetProductsAsync(10, 0, null, Array.Empty<int?>()))
                    .ReturnsAsync((entities, 1));
            mockMapper.Setup(m => m.Map<IEnumerable<ProductDTO>>(entities)).Returns(dtos);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            await svc.GetProductsAsync(10, 0, null, []);

            mockCache.Verify(c => c.SetProductListAsync("test-key", It.IsAny<IEnumerable<ProductDTO>>(), 1), Times.Once);
        }

        [Fact]
        public async Task AddProductAsync_InvalidatesListCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            var dto = new AdminProductDTO(null, 1, "NewProduct", 10, "p");
            var entity = new Product { ProductId = 99, ProductName = "NewProduct", ProductPrompt = "p" };
            var resultDto = new ProductDTO(99, 1, "NewProduct", "Sub", 10, "p");
            mockMapper.Setup(m => m.Map<Product>(dto)).Returns(entity);
            mockRepo.Setup(r => r.AddProductAsync(It.IsAny<Product>())).ReturnsAsync(entity);
            mockMapper.Setup(m => m.Map<ProductDTO>(entity)).Returns(resultDto);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            await svc.AddProductAsync(dto);

            mockCache.Verify(c => c.InvalidateProductListsAsync(), Times.Once);
            mockCache.Verify(c => c.InvalidateProductAsync(It.IsAny<long>()), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_InvalidatesProductAndListCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            var dto = new AdminProductDTO(7, 1, "Updated", 20, "p");
            var entity = new Product { ProductId = 7, ProductName = "Updated", ProductPrompt = "p" };
            mockMapper.Setup(m => m.Map<Product>(dto)).Returns(entity);
            mockRepo.Setup(r => r.UpdateProductAsync(7, It.IsAny<Product>())).Returns(Task.CompletedTask);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            await svc.UpdateProductAsync(7, dto);

            mockCache.Verify(c => c.InvalidateProductAsync(7), Times.Once);
            mockCache.Verify(c => c.InvalidateProductListsAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenDeleted_InvalidatesProductAndListCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            mockRepo.Setup(r => r.GetProductByIdAsync(3)).ReturnsAsync(new Product { ProductId = 3, ProductName = "P" });
            mockRepo.Setup(r => r.HasOrderItemsByProductIdAsync(3)).ReturnsAsync(false);
            mockRepo.Setup(r => r.RemoveCartItemsByProductIdAsync(3)).Returns(Task.CompletedTask);
            mockRepo.Setup(r => r.DeleteProductAsync(3)).ReturnsAsync(true);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            await svc.DeleteProductAsync(3);

            mockCache.Verify(c => c.InvalidateProductAsync(3), Times.Once);
            mockCache.Verify(c => c.InvalidateProductListsAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_WhenNotFound_DoesNotInvalidateCache()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCache = BuildNoOpCache();

            mockRepo.Setup(r => r.GetProductByIdAsync(99)).ReturnsAsync((Product)null!);

            var svc = new ProductService(mockRepo.Object, mockMapper.Object, mockCache.Object);

            await svc.DeleteProductAsync(99);

            mockCache.Verify(c => c.InvalidateProductAsync(It.IsAny<long>()), Times.Never);
            mockCache.Verify(c => c.InvalidateProductListsAsync(), Times.Never);
        }

        #endregion
    }
}
