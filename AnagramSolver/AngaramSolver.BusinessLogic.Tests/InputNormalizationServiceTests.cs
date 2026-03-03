using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace AnagramSolver.BusinessLogic.Tests
{
    public class InputNormalizationServiceTests
    {
        private readonly Mock<IWordProcessor> _mockWordProcessor;
        private readonly InputNormalizationService _sut;

        public InputNormalizationServiceTests()
        {
            _mockWordProcessor = new Mock<IWordProcessor>();
            _sut = new InputNormalizationService(_mockWordProcessor.Object);
        }

        [Fact]
        public void NormalizeInput_ShouldCallWordProcessorMethods_AndReturnCorrectResult()
        {
            // Arrange
            var input = " some input ";
            var cleanedInput = "someinput";
            var expectedResult = new Dictionary<char, int>
            {
                { 's', 1 }, { 'o', 1 }, { 'm', 1 }, { 'e', 1 },
                { 'i', 1 }, { 'n', 1 }, { 'p', 1 }, { 'u', 1 }, { 't', 1 }
            };

            _mockWordProcessor.Setup(x => x.RemoveWhitespace(input))
                .Returns(cleanedInput);
            
            _mockWordProcessor.Setup(x => x.CreateCharCount(cleanedInput))
                .Returns(expectedResult);

            // Act
            var result = _sut.NormalizeInput(input);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
            _mockWordProcessor.Verify(x => x.RemoveWhitespace(input), Times.Once);
            _mockWordProcessor.Verify(x => x.CreateCharCount(cleanedInput), Times.Once);
        }
    }
}
