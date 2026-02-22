using AutoMapper;
using DTO;
using Entities;
using Moq;
using Repositories;
using Services;

namespace Tests.UnitTests
{
    public class MainCategoryServiceUnitTests
    {
        [Fact]
        public async Task AddMainCategoryAsync_WithEmptyPrompt_ThrowsArgumentException()
        {
            var repository = new Mock<IMainCategoryRepository>();
            var mapper = new Mock<IMapper>();
            var service = new MainCategoryService(repository.Object, mapper.Object);

            await Assert.ThrowsAsync<ArgumentException>(() =>
                service.AddMainCategoryAsync(new AdminMainCategoryDTO("Business", " ")));
        }

        [Fact]
        public async Task UpdateMainCategoryAsync_WhenExistsAndPromptMissing_PreservesExistingPrompt()
        {
            var repository = new Mock<IMainCategoryRepository>();
            var mapper = new Mock<IMapper>();

            repository.Setup(r => r.GetMainCategoryByIdAsync(5)).ReturnsAsync(new MainCategory
            {
                MainCategoryId = 5,
                MainCategoryName = "Old",
                MainCategoryPrompt = "keep-me"
            });

            mapper.Setup(m => m.Map<MainCategory>(It.IsAny<AdminMainCategoryDTO>())).Returns(new MainCategory
            {
                MainCategoryName = "New"
            });

            var service = new MainCategoryService(repository.Object, mapper.Object);

            var result = await service.UpdateMainCategoryAsync(5, new AdminMainCategoryDTO("New", ""));

            Assert.True(result);
            repository.Verify(r => r.UpdateMainCategoryAsync(It.Is<MainCategory>(x =>
                x.MainCategoryId == 5 && x.MainCategoryPrompt == "keep-me")), Times.Once);
        }
    }
}
