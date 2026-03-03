using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests
{
    public class AnagramSolverTests
    {
        private readonly Mock<IMemoryCache<IEnumerable<string>>> _mockMemoryCache;
        private readonly Mock<ISearchLogRepository> _mockSearchLogRepository;
        private readonly Mock<IInputNormalization> _mockInputNormalization;
        private readonly Mock<IAnagramFinder> _mockAnagramFinder;
        private readonly AnagramSolverService _sut;

        public AnagramSolverTests()
        {
            _mockMemoryCache = new Mock<IMemoryCache<IEnumerable<string>>>();
            _mockSearchLogRepository = new Mock<ISearchLogRepository>();
            _mockInputNormalization = new Mock<IInputNormalization>();
            _mockAnagramFinder = new Mock<IAnagramFinder>();

            _sut = new AnagramSolverService(
                _mockMemoryCache.Object,
                _mockSearchLogRepository.Object,
                _mockInputNormalization.Object,
                _mockAnagramFinder.Object
            );
        }

        [Fact]
        public async Task GetAnagramsAsync_ShouldReturnCachedValue_WhenCacheContainsInput()
        {
            // Arrange
            var input = "test";
            IEnumerable<string> cachedValue = new List<string> { "sets", "tset" };
            
            _mockMemoryCache.Setup(x => x.TryGet(input, out cachedValue))
                .Returns(true);

            // Act
            var result = await _sut.GetAnagramsAsync(input, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(cachedValue);
            _mockAnagramFinder.Verify(x => x.FindAnagramsAsync(It.IsAny<Dictionary<char, int>>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetAnagramsAsync_ShouldCallFinderAndCacheResult_WhenCacheMiss()
        {
            // Arrange
            var input = "test";
            IEnumerable<string> cachedValue = null;
            var foundAnagrams = new List<string> { "sets", "tset" };
            var charCount = new Dictionary<char, int>();

            _mockMemoryCache.Setup(x => x.TryGet(input, out cachedValue))
                .Returns(false);

            _mockInputNormalization.Setup(x => x.NormalizeInput(input))
                .Returns(charCount);

            _mockAnagramFinder.Setup(x => x.FindAnagramsAsync(charCount, input, It.IsAny<CancellationToken>()))
                .ReturnsAsync(foundAnagrams);

            // Act
            var result = await _sut.GetAnagramsAsync(input, CancellationToken.None);

            // Assert
            result.Should().BeEquivalentTo(foundAnagrams);
            _mockMemoryCache.Verify(x => x.Add(input, foundAnagrams), Times.Once);
            _mockSearchLogRepository.Verify(x => x.AddSearchLogAsync(input, foundAnagrams.Count, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
