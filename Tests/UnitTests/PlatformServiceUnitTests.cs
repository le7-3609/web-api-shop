using AutoMapper;
using DTO;
using Entities;
using Moq;
using Repositories;
using Services;

namespace Tests.UnitTests
{
    public class PlatformServiceUnitTests
    {
        [Fact]
        public async Task AddPlatformAsync_WhenNameExists_ReturnsNull()
        {
            var repository = new Mock<IPlatformRepository>();
            var mapper = new Mock<IMapper>();
            repository.Setup(r => r.GetPlatformByNameAsync("Webflow")).ReturnsAsync(new Platform { PlatformId = 1, PlatformName = "Webflow" });

            var service = new PlatformService(repository.Object, mapper.Object);

            var result = await service.AddPlatformAsync("Webflow");

            Assert.Null(result);
            repository.Verify(r => r.AddPlatformAsync(It.IsAny<Platform>()), Times.Never);
        }

        [Fact]
        public async Task AddPlatformAsync_WhenNewName_ReturnsMappedDto()
        {
            var repository = new Mock<IPlatformRepository>();
            var mapper = new Mock<IMapper>();

            repository.Setup(r => r.GetPlatformByNameAsync("Framer")).ReturnsAsync((Platform)null!);
            var saved = new Platform { PlatformId = 11, PlatformName = "Framer" };
            repository.Setup(r => r.AddPlatformAsync(It.IsAny<Platform>())).ReturnsAsync(saved);
            mapper.Setup(m => m.Map<PlatformsDTO>(saved)).Returns(new PlatformsDTO(11, "Framer", ""));

            var service = new PlatformService(repository.Object, mapper.Object);

            var result = await service.AddPlatformAsync("Framer");

            Assert.NotNull(result);
            Assert.Equal(11, result.PlatformId);
        }
    }
}
