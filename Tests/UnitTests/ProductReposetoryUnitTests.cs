using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Entities;
using Xunit;
using Microsoft.EntityFrameworkCore;

namespace Tests.UnitTests
{
    public class ProductRepositoryUnitTesting
    {
        private readonly Mock<MyShopContext> _mockContext;
        private readonly ProductRepository _repo;

        public ProductRepositoryUnitTesting()
        {
            _mockContext = new Mock<MyShopContext>();
            _repo = new ProductRepository(_mockContext.Object);
        }

        // פונקציית עזר ליצירת מוצר תקין עם כל שדות החובה
        private Product CreateValidProduct(int id, string name = "Test") =>
            new Product { ProductId = id, ProductName = name, ProductPrompt = "Required Content", SubCategoryId = 1 };

        // פונקציית עזר לאתחול ה-DbSets כדי למנוע NullReference
        private void MockDbSets(List<Product> products = null, List<CartItem> carts = null, List<OrderItem> orders = null)
        {
            _mockContext.Setup(x => x.Products).ReturnsDbSet(products ?? new List<Product>());
            _mockContext.Setup(x => x.CartItems).ReturnsDbSet(carts ?? new List<CartItem>());
            _mockContext.Setup(x => x.OrderItems).ReturnsDbSet(orders ?? new List<OrderItem>());
        }

        #region Happy Paths

        [Fact]
        public async Task AddProductAsync_ValidProduct_ReturnsProduct()
        {
            MockDbSets();
            var product = CreateValidProduct(0, "New Product");

            var result = await _repo.AddProductAsync(product);

            Assert.Equal("New Product", result.ProductName);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductExistsAndNoLinks_ReturnsTrue()
        {
            var product = CreateValidProduct(10);
            MockDbSets(products: new List<Product> { product });

            var result = await _repo.DeleteProductAsync(10);

            Assert.True(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task DeleteProductAsync_ProductInOrder_ReturnsFalse()
        {
            // Arrange: מוצר שקיים במערכת אבל מופיע בהזמנה
            var product = CreateValidProduct(1);
            var orderItem = new OrderItem { PlatformId = 1 };
            MockDbSets(products: new List<Product> { product }, orders: new List<OrderItem> { orderItem });

            // Act
            var result = await _repo.DeleteProductAsync(1);

            // Assert
            Assert.False(result); // אסור למחוק מוצר שמופיע בהזמנות
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task DeleteProductAsync_ProductInCart_ReturnsFalse()
        {
            // Arrange: מוצר שקיים בסל קניות של מישהו
            var product = CreateValidProduct(1);
            var cartItem = new CartItem { PlatformId = 1 };
            MockDbSets(products: new List<Product> { product }, carts: new List<CartItem> { cartItem });

            // Act
            var result = await _repo.DeleteProductAsync(1);

            // Assert
            Assert.False(result);
            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Never);
        }

        [Fact]
        public async Task UpdateProductAsync_InvalidId_StillCallsSave()
        {
            // הערה: לפי המימוש שלך, ה-Update פשוט קורא ל-Save מבלי לבדוק קיום
            MockDbSets();
            var product = CreateValidProduct(999);

            await _repo.UpdateProductAsync(999, product);

            _mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        #endregion
    }
}