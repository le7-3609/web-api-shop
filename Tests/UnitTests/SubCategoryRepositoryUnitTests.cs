using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Entities;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests.UnitTests
{
    public class SubCategoryRepositoryUnitTests
    {
        #region Happy Paths

        [Fact]
        public async Task GetSubCategoryByIdAsync_ExistingId_ReturnsSubCategory()
        {
            // Arrange
            var subCategories = new List<SubCategory> { new SubCategory { SubCategoryId = 1, SubCategoryName = "Laptops" } };
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(subCategories);
            var repo = new SubCategoryRepository(mockContext.Object);

            // Act
            var result = await repo.GetSubCategoryByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Laptops", result.SubCategoryName);
        }

        [Fact]
        public async Task GetSubCategoryAsync_FilterByDescription_ReturnsMatchingItems()
        {
            // Arrange
            var subCategories = new List<SubCategory>
            {
                new SubCategory { SubCategoryName = "S1", CategoryDescription = "Apple Device", MainCategoryId = 1 },
                new SubCategory { SubCategoryName = "S2", CategoryDescription = "Samsung Device", MainCategoryId = 1 }
            };
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(subCategories);
            var repo = new SubCategoryRepository(mockContext.Object);

            // Act
            var (results, total) = await repo.GetSubCategoryAsync(1, 10, "Apple", Array.Empty<int?>());

            // Assert
            Assert.Equal(1, total);
            Assert.Equal("S1", results.First().SubCategoryName);
        }

        [Fact]
        public async Task AddSubCategoryAsync_ValidSubCategory_SavesAndReturns()
        {
            // Arrange
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(new List<SubCategory>());
            var repo = new SubCategoryRepository(mockContext.Object);
            var newSub = new SubCategory { SubCategoryName = "Hardware" };

            // Act
            var result = await repo.AddSubCategoryAsync(newSub);

            // Assert
            Assert.Equal("Hardware", result.SubCategoryName);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task UpdateSubCategoryAsync_ValidData_CallsUpdateAndSave()
        {
            // Arrange
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(new List<SubCategory>());
            var repo = new SubCategoryRepository(mockContext.Object);
            var subToUpdate = new SubCategory { SubCategoryId = 1, SubCategoryName = "Updated Name" };

            // Act
            await repo.UpdateSubCategoryAsync(1, subToUpdate);

            // Assert
            mockContext.Verify(m => m.SubCategories.Update(subToUpdate), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        [Fact]
        public async Task DeleteSubCategoryAsync_NoLinkedProducts_ReturnsTrue()
        {
            // Arrange
            var sub = new SubCategory { SubCategoryId = 1 };
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(new List<SubCategory> { sub });
            mockContext.Setup(x => x.Products).ReturnsDbSet(new List<Product>());
            var repo = new SubCategoryRepository(mockContext.Object);

            // Act
            var result = await repo.DeleteSubCategoryAsync(1);

            // Assert
            Assert.True(result);
            mockContext.Verify(m => m.SubCategories.Remove(sub), Times.Once);
            mockContext.Verify(m => m.SaveChangesAsync(default), Times.Once);
        }

        #endregion

        #region Unhappy Paths

        [Fact]
        public async Task GetSubCategoryByIdAsync_NonExistingId_ReturnsNull()
        {
            // Arrange
            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(new List<SubCategory>());
            var repo = new SubCategoryRepository(mockContext.Object);

            // Act
            var result = await repo.GetSubCategoryByIdAsync(999);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task DeleteSubCategoryAsync_WithLinkedProducts_ReturnsFalse()
        {
            // Arrange
            var subId = 1;
            var sub = new SubCategory { SubCategoryId = subId };
            var products = new List<Product> { new Product { ProductId = 10, SubCategoryId = subId } };

            var mockContext = new Mock<MyShopContext>();
            mockContext.Setup(x => x.SubCategories).ReturnsDbSet(new List<SubCategory> { sub });
            mockContext.Setup(x => x.Products).ReturnsDbSet(products);
            var repo = new SubCategoryRepository(mockContext.Object);

            // Act
            var result = await repo.DeleteSubCategoryAsync(subId);

            // Assert
            Assert.False(result);
            mockContext.Verify(m => m.SubCategories.Remove(It.IsAny<SubCategory>()), Times.Never);
        }

        #endregion
    }
}