using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests
{
    public class SimpleAnagramAlgorithmTests
    {
        private readonly Mock<IWordProcessor> _wordProcessorMock;
        private readonly SimpleAnagramAlgorithm _algorithm;

        public SimpleAnagramAlgorithmTests()
        {
            _wordProcessorMock = new Mock<IWordProcessor>();
            _algorithm = new SimpleAnagramAlgorithm(_wordProcessorMock.Object);
        }

        [Fact]
        public void GetAnagrams_WhenAnagramCountIsLessThanOne_ReturnsEmptyList()
        {
            // Arrange
            var targetLetters = new Dictionary<char, int> { { 'a', 1 } };
            var allAnagrams = new List<Anagram>();
            var anagramCount = 0;
            var minOutputWordsLength = 0;

            // Act
            var result = _algorithm.GetAnagrams(targetLetters, anagramCount, allAnagrams, minOutputWordsLength);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAnagrams_WhenAnagramCountIsOne_AndKeyMatches_ReturnsMatchingWords()
        {
            // Arrange
            var targetLetters = new Dictionary<char, int> { { 'a', 1 }, { 'b', 1 } };
            var anagramCount = 1;
            var minOutputWordsLength = 0;
            var sortedKey = "ab";

            _wordProcessorMock.Setup(x => x.SortString(It.IsAny<string>()))
                .Returns(sortedKey);

            var allAnagrams = new List<Anagram>
            {
                new Anagram
                {
                    Key = sortedKey,
                    Words = new List<string> { "ab", "ba" }
                },
                new Anagram
                {
                    Key = "other",
                    Words = new List<string> { "other" }
                }
            };

            // Act
            var result = _algorithm.GetAnagrams(targetLetters, anagramCount, allAnagrams, minOutputWordsLength);

            // Assert
            result.Should().HaveCount(2);
            result.Should().BeEquivalentTo("ab", "ba");
        }

        [Fact]
        public void GetAnagrams_WhenAnagramCountIsOne_AndKeyDoesNotMatch_ReturnsEmptyList()
        {
            // Arrange
            var targetLetters = new Dictionary<char, int> { { 'x', 1 } };
            var anagramCount = 1;
            var minOutputWordsLength = 0;
            var sortedKey = "x";

            _wordProcessorMock.Setup(x => x.SortString(It.IsAny<string>()))
                .Returns(sortedKey);

            var allAnagrams = new List<Anagram>
            {
                new Anagram
                {
                    Key = "y",
                    Words = new List<string> { "y" }
                }
            };

            // Act
            var result = _algorithm.GetAnagrams(targetLetters, anagramCount, allAnagrams, minOutputWordsLength);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAnagrams_WhenAnagramCountIsGreaterThanOne_ReturnsEmptyList()
        {
            // Arrange
            var targetLetters = new Dictionary<char, int> { { 'a', 1 } };
            var allAnagrams = new List<Anagram>
            {
                new Anagram
                {
                    Key = "a",
                    Words = new List<string> { "a" }
                }
            };
            var anagramCount = 2; // Simple algorithm does not handle counts > 1
            var minOutputWordsLength = 0;

            // Act
            var result = _algorithm.GetAnagrams(targetLetters, anagramCount, allAnagrams, minOutputWordsLength);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void GetAnagrams_ShouldCallWordProcessorSortString_ToCreateKey()
        {
            // Arrange
            var targetLetters = new Dictionary<char, int> { { 'a', 1 } };
            var anagramCount = 1;
            var allAnagrams = new List<Anagram>();
            var minOutputWordsLength = 0;

            _wordProcessorMock.Setup(x => x.SortString(It.IsAny<string>()))
                .Returns("a");

            // Act
            _algorithm.GetAnagrams(targetLetters, anagramCount, allAnagrams, minOutputWordsLength);

            // Assert
            _wordProcessorMock.Verify(x => x.SortString(It.IsAny<string>()), Times.Once);
        }
    }
}