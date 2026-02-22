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
        [Fact]
        public async Task AddProductAsync_Throws_OnEmptyName()
        {
            var mockRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var svc = new ProductService(mockRepo.Object, mockMapper.Object);

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

            var svc = new ProductService(mockRepo.Object, mockMapper.Object);

            var res = await svc.AddProductAsync(dto);

            mockRepo.Verify(r => r.AddProductAsync(It.IsAny<Product>()), Times.Once);
            Assert.Equal(outDto.ProductId, res.ProductId);
        }
    }
}
