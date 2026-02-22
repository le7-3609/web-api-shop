using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DTO;
using Entities;
using Moq;
using Repositories;
using Services;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Tests.UnitTests
{
    public class OrderServiceUnitTests
    {
        [Fact]
        public async Task AddOrderFromCartAsync_InvalidCartId_Throws()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddOrderFromCartAsync(0));
        }

        [Fact]
        public async Task AddOrderFromCartAsync_CartNotFound_Throws()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            mockCartService.Setup(c => c.GetCartByIdAsync(5)).ReturnsAsync((CartDTO?)null);

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => svc.AddOrderFromCartAsync(5));
        }

        [Fact]
        public async Task AddOrderFromCartAsync_MissingBasicSiteId_Throws()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            mockCartService.Setup(c => c.GetCartByIdAsync(5)).ReturnsAsync(new CartDTO
            {
                CartId = 5,
                UserId = 7,
                BasicSiteId = null,
                CartItems = new List<CartItemDTO> { new CartItemDTO { ProductId = 1 } }
            });

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddOrderFromCartAsync(5));
        }

        [Fact]
        public async Task AddOrderFromCartAsync_ProductMissing_ThrowsKeyNotFound()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            mockCartService.Setup(c => c.GetCartByIdAsync(5)).ReturnsAsync(new CartDTO
            {
                CartId = 5,
                UserId = 7,
                BasicSiteId = 2,
                BasicSitePrice = 15,
                TotalPrice = 25,
                CartItems = new List<CartItemDTO>
                {
                    new CartItemDTO { ProductId = 999, PlatformId = 1 }
                }
            });
            mockProductRepo.Setup(p => p.GetProductByIdAsync(999)).ReturnsAsync((Product?)null);

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<KeyNotFoundException>(() => svc.AddOrderFromCartAsync(5));
        }

        [Fact]
        public async Task AddOrderFromCartAsync_NullPlatformId_DefaultsToOneInOrderItems()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            Order? capturedOrder = null;
            mockOrderRepo.Setup(r => r.AddOrderAsync(It.IsAny<Order>()))
                .Callback<Order>(order => capturedOrder = order)
                .ReturnsAsync((Order order) =>
                {
                    order.OrderId = 42;
                    return order;
                });

            mockMapper.Setup(m => m.Map<OrderDetailsDTO>(It.IsAny<Order>()))
                .Returns((Order order) => new OrderDetailsDTO { OrderId = order.OrderId, OrderSum = order.OrderSum });

            mockCartService.Setup(c => c.GetCartByIdAsync(5)).ReturnsAsync(new CartDTO
            {
                CartId = 5,
                UserId = 7,
                BasicSiteId = 2,
                BasicSitePrice = 10,
                TotalPrice = 40,
                CartItems = new List<CartItemDTO>
                {
                    new CartItemDTO { ProductId = 1, PlatformId = null },
                    new CartItemDTO { ProductId = 2, PlatformId = 3 }
                }
            });

            mockProductRepo.Setup(p => p.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, Price = 10 });
            mockProductRepo.Setup(p => p.GetProductByIdAsync(2)).ReturnsAsync(new Product { ProductId = 2, Price = 20 });

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);
            await svc.AddOrderFromCartAsync(5);

            Assert.NotNull(capturedOrder);
            Assert.Equal(1, capturedOrder!.OrderItems.First().PlatformId);
        }

        [Fact]
        public async Task AddOrderFromCartAsync_SucceedsAndClearsCart()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            const int cartId = 5;

            // products with prices
            mockProductRepo.Setup(p => p.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, Price = 10 });
            mockProductRepo.Setup(p => p.GetProductByIdAsync(2)).ReturnsAsync(new Product { ProductId = 2, Price = 20 });

            var cartDto = new CartDTO { CartId = 5, UserId = 7, BasicSitePrice = 15, TotalPrice = 45, CartItems = new List<CartItemDTO>
            {
                new CartItemDTO { ProductId = 1, PlatformId = 1, UserDescription = "a" },
                new CartItemDTO { ProductId = 2, PlatformId = 1, UserDescription = "b" }
            }, BasicSiteId = 2 };

            mockCartService.Setup(c => c.GetCartByIdAsync(cartId)).ReturnsAsync(cartDto);

            var createdOrder = new Order { OrderId = 42, OrderSum = 45 };
            mockOrderRepo.Setup(r => r.AddOrderAsync(It.IsAny<Order>())).ReturnsAsync(createdOrder);

            var expectedDto = new OrderDetailsDTO { OrderId = 42, OrderSum = 45 };
            mockMapper.Setup(m => m.Map<OrderDetailsDTO>(It.IsAny<Order>())).Returns(expectedDto);

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            var result = await svc.AddOrderFromCartAsync(cartId);

            mockOrderRepo.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Once);
            mockCartService.Verify(c => c.ClearCartAsync(cartId), Times.Once);
            Assert.Equal(expectedDto.OrderId, result.OrderId);
            Assert.Equal(expectedDto.OrderSum, result.OrderSum);
        }

        [Fact]
        public async Task AddOrderFromCartAsync_EmptyCart_Throws()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            mockCartService.Setup(c => c.GetCartByIdAsync(5)).ReturnsAsync(new CartDTO
            {
                CartId = 5,
                UserId = 7,
                BasicSiteId = 2,
                CartItems = new List<CartItemDTO>()
            });

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddOrderFromCartAsync(5));

            mockOrderRepo.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Never);
            mockCartService.Verify(c => c.ClearCartAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task AddOrderFromCartAsync_TotalMismatch_Throws()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            const int cartId = 5;

            mockProductRepo.Setup(p => p.GetProductByIdAsync(1)).ReturnsAsync(new Product { ProductId = 1, Price = 10 });
            mockProductRepo.Setup(p => p.GetProductByIdAsync(2)).ReturnsAsync(new Product { ProductId = 2, Price = 20 });

            mockCartService.Setup(c => c.GetCartByIdAsync(cartId)).ReturnsAsync(new CartDTO
            {
                CartId = cartId,
                UserId = 7,
                BasicSiteId = 2,
                BasicSitePrice = 15,
                TotalPrice = 999,
                CartItems = new List<CartItemDTO>
                {
                    new CartItemDTO { ProductId = 1, PlatformId = 1 },
                    new CartItemDTO { ProductId = 2, PlatformId = 1 }
                }
            });

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() => svc.AddOrderFromCartAsync(cartId));

            mockOrderRepo.Verify(r => r.AddOrderAsync(It.IsAny<Order>()), Times.Never);
            mockCartService.Verify(c => c.ClearCartAsync(It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task GetByIdAsync_NotFound_ReturnsNull()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            mockOrderRepo.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((Order?)null);

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            var res = await svc.GetByIdAsync(123);
            Assert.Null(res);
        }

        [Fact]
        public async Task UpdateStatusAsync_CallsRepository()
        {
            var mockOrderRepo = new Mock<IOrderRepository>();
            var mockProductRepo = new Mock<IProductRepository>();
            var mockMapper = new Mock<IMapper>();
            var mockCartService = new Mock<ICartService>();
            var mockLogger = new Mock<ILogger<OrderService>>();

            var dto = new OrderSummaryDTO { OrderId = 9, OrderSum = 10 };
            mockMapper.Setup(m => m.Map<Order>(dto)).Returns(new Order { OrderId = 9 });

            var svc = new OrderService(mockOrderRepo.Object, mockMapper.Object, mockCartService.Object, mockLogger.Object, mockProductRepo.Object);

            await svc.UpdateStatusAsync(dto);

            mockOrderRepo.Verify(r => r.UpdateStatusAsync(It.Is<Order>(o => o.OrderId == 9)), Times.Once);
        }
    }
}
