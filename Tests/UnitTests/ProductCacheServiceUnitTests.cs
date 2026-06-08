using System.Text.Json;
using DTO;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Services;
using StackExchange.Redis;
using Xunit;

namespace Tests.UnitTests
{
    public class ProductCacheServiceUnitTests
    {
        private readonly Mock<IDatabase> _mockDb;
        private readonly Mock<IConnectionMultiplexer> _mockMultiplexer;
        private readonly ProductCacheService _svc;

        private static readonly ProductDTO SampleDto =
            new(1, 2, "Widget", "Sub", 10.0, "prompt");

        public ProductCacheServiceUnitTests()
        {
            _mockDb = new Mock<IDatabase>();
            _mockMultiplexer = new Mock<IConnectionMultiplexer>();
            _mockMultiplexer
                .Setup(m => m.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
                .Returns(_mockDb.Object);

            _svc = new ProductCacheService(_mockMultiplexer.Object, NullLogger<ProductCacheService>.Instance);
        }

        #region GetProductAsync

        [Fact]
        public async Task GetProductAsync_CacheMiss_ReturnsNull()
        {
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync(RedisValue.Null);

            var result = await _svc.GetProductAsync(1);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetProductAsync_CacheHit_ReturnsDeserializedDto()
        {
            var json = JsonSerializer.Serialize(SampleDto);
            _mockDb.Setup(d => d.StringGetAsync("product:1", It.IsAny<CommandFlags>()))
                   .ReturnsAsync((RedisValue)json);

            var result = await _svc.GetProductAsync(1);

            Assert.NotNull(result);
            Assert.Equal(SampleDto.ProductId, result!.ProductId);
            Assert.Equal(SampleDto.ProductName, result.ProductName);
        }

        [Fact]
        public async Task GetProductAsync_RedisException_ReturnsNullWithoutThrowing()
        {
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("connection refused"));

            var result = await _svc.GetProductAsync(1);

            Assert.Null(result);
        }

        #endregion

        #region SetProductAsync

        [Fact]
        public async Task SetProductAsync_CallsStringSetWithCorrectKey()
        {
            _mockDb.Setup(d => d.StringSetAsync(
                       It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                       It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync(true);

            await _svc.SetProductAsync(1, SampleDto);

            _mockDb.Verify(d => d.StringSetAsync(
                "product:1", It.IsAny<RedisValue>(),
                TimeSpan.FromMinutes(10), It.IsAny<When>(), It.IsAny<CommandFlags>()),
                Times.Once);
        }

        [Fact]
        public async Task SetProductAsync_RedisException_DoesNotThrow()
        {
            _mockDb.Setup(d => d.StringSetAsync(
                       It.IsAny<RedisKey>(), It.IsAny<RedisValue>(),
                       It.IsAny<TimeSpan?>(), It.IsAny<When>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("timeout"));

            var exception = await Record.ExceptionAsync(() => _svc.SetProductAsync(1, SampleDto));
            Assert.Null(exception);
        }

        #endregion

        #region InvalidateProductAsync

        [Fact]
        public async Task InvalidateProductAsync_CallsKeyDeleteWithCorrectKey()
        {
            _mockDb.Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync(true);

            await _svc.InvalidateProductAsync(5);

            _mockDb.Verify(d => d.KeyDeleteAsync("product:5", It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task InvalidateProductAsync_RedisException_DoesNotThrow()
        {
            _mockDb.Setup(d => d.KeyDeleteAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("timeout"));

            var exception = await Record.ExceptionAsync(() => _svc.InvalidateProductAsync(1));
            Assert.Null(exception);
        }

        #endregion

        #region InvalidateProductListsAsync

        [Fact]
        public async Task InvalidateProductListsAsync_IncrementsVersionKey()
        {
            _mockDb.Setup(d => d.StringIncrementAsync(It.IsAny<RedisKey>(), It.IsAny<long>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync(1L);

            await _svc.InvalidateProductListsAsync();

            _mockDb.Verify(d => d.StringIncrementAsync("products:version", 1, It.IsAny<CommandFlags>()), Times.Once);
        }

        [Fact]
        public async Task InvalidateProductListsAsync_RedisException_DoesNotThrow()
        {
            _mockDb.Setup(d => d.StringIncrementAsync(It.IsAny<RedisKey>(), It.IsAny<long>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("timeout"));

            var exception = await Record.ExceptionAsync(() => _svc.InvalidateProductListsAsync());
            Assert.Null(exception);
        }

        #endregion

        #region GetProductListAsync

        [Fact]
        public async Task GetProductListAsync_CacheMiss_ReturnsNullItems()
        {
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync(RedisValue.Null);

            var (items, total) = await _svc.GetProductListAsync("products:v1:10:0::");

            Assert.Null(items);
            Assert.Equal(0, total);
        }

        [Fact]
        public async Task GetProductListAsync_CacheHit_ReturnsDeserializedList()
        {
            var payload = JsonSerializer.Serialize(new { Items = new[] { SampleDto }, TotalCount = 1 });
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ReturnsAsync((RedisValue)payload);

            var (items, total) = await _svc.GetProductListAsync("products:v1:10:0::");

            Assert.NotNull(items);
            Assert.Equal(1, total);
            Assert.Single(items!);
        }

        [Fact]
        public async Task GetProductListAsync_RedisException_ReturnsNullWithoutThrowing()
        {
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("connection refused"));

            var (items, total) = await _svc.GetProductListAsync("products:v1:10:0::");

            Assert.Null(items);
            Assert.Equal(0, total);
        }

        #endregion

        #region BuildListCacheKeyAsync

        [Fact]
        public async Task BuildListCacheKeyAsync_IncludesVersionAndAllParams()
        {
            _mockDb.Setup(d => d.StringGetAsync("products:version", It.IsAny<CommandFlags>()))
                   .ReturnsAsync((RedisValue)"3");

            var key = await _svc.BuildListCacheKeyAsync(10, 20, "widget", [1, 2]);

            Assert.StartsWith("products:v3:", key);
            Assert.Contains("10", key);
            Assert.Contains("20", key);
            Assert.Contains("widget", key);
        }

        [Fact]
        public async Task BuildListCacheKeyAsync_WhenVersionKeyMissing_UsesVersionZero()
        {
            _mockDb.Setup(d => d.StringGetAsync("products:version", It.IsAny<CommandFlags>()))
                   .ReturnsAsync(RedisValue.Null);

            var key = await _svc.BuildListCacheKeyAsync(10, 0, null, []);

            Assert.StartsWith("products:v0:", key);
        }

        [Fact]
        public async Task BuildListCacheKeyAsync_RedisException_UsesVersionZero()
        {
            _mockDb.Setup(d => d.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()))
                   .ThrowsAsync(new RedisException("timeout"));

            var key = await _svc.BuildListCacheKeyAsync(10, 0, null, []);

            Assert.StartsWith("products:v0:", key);
        }

        #endregion
    }
}
