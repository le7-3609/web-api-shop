using AutoMapper;
using DTO;
using Entities;
using Moq;
using Repositories;
using Services;

namespace Tests.UnitTests
{
    public class SubCategoryServiceUnitTests
    {
        [Fact]
        public async Task AddSubCategoryAsync_WhenMainCategoryMissing_ThrowsInvalidOperationException()
        {
            var repository = new Mock<ISubCategoryRepository>();
            var mapper = new Mock<IMapper>();

            repository.Setup(r => r.GetByNameAsync("Landing")).ReturnsAsync((SubCategory)null!);
            repository.Setup(r => r.MainCategoryExistsAsync(99)).ReturnsAsync(false);

            var service = new SubCategoryService(repository.Object, mapper.Object);

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                service.AddSubCategoryAsync(new AddSubCategoryDTO(99, "Landing", "p", "img", "desc")));
        }

        [Fact]
        public async Task AddSubCategoryAsync_WhenValid_AddsAndReturnsDto()
        {
            var repository = new Mock<ISubCategoryRepository>();
            var mapper = new Mock<IMapper>();

            var dto = new AddSubCategoryDTO(1, "Landing", "p", "img", "desc");
            var mapped = new SubCategory { MainCategoryId = 1, SubCategoryName = "Landing" };
            var saved = new SubCategory { SubCategoryId = 10, MainCategoryId = 1, SubCategoryName = "Landing", SubCategoryPrompt = "gfasdfghfh", ImageUrl = "img", CategoryDescription = "desc" };
            var mappedDto = new SubCategoryDTO(10, 1, "Landing", "gfasdfghfh", "img", "desc");

            repository.Setup(r => r.GetByNameAsync("Landing")).ReturnsAsync((SubCategory)null!);
            repository.Setup(r => r.MainCategoryExistsAsync(1)).ReturnsAsync(true);
            mapper.Setup(m => m.Map<SubCategory>(dto)).Returns(mapped);
            repository.Setup(r => r.AddSubCategoryAsync(It.IsAny<SubCategory>())).ReturnsAsync(saved);
            mapper.Setup(m => m.Map<SubCategoryDTO>(saved)).Returns(mappedDto);

            var service = new SubCategoryService(repository.Object, mapper.Object);

            var result = await service.AddSubCategoryAsync(dto);

            Assert.Equal(10, result.SubCategoryId);
            repository.Verify(r => r.AddSubCategoryAsync(It.Is<SubCategory>(x => x.SubCategoryPrompt == "gfasdfghfh")), Times.Once);
        }
    }
}
