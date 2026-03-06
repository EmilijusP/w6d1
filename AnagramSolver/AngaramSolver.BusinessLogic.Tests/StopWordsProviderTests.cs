using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;

namespace AnagramSolver.BusinessLogic.Tests;

public class StopWordsProviderTests
{
    [Fact]
    public void GetStopWords_ConfigContainsWords_ReturnsNormalizedSet()
    {
        // Arrange
        var options = Options.Create(new FrequencyAnalysisOptions
        {
            StopWords = ["The", "AND", "or"]
        });
        var provider = new StopWordsProvider(options);

        // Act
        var result = provider.GetStopWords();

        // Assert
        result.Should().HaveCount(3);
        result.Should().Contain("the");
        result.Should().Contain("and");
        result.Should().Contain("or");
    }

    [Fact]
    public void GetStopWords_DuplicatesPresent_ReturnsDistinctEntries()
    {
        // Arrange
        var options = Options.Create(new FrequencyAnalysisOptions
        {
            StopWords = ["the", "THE", "The", "and", "AND"]
        });
        var provider = new StopWordsProvider(options);

        // Act
        var result = provider.GetStopWords();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("the");
        result.Should().Contain("and");
    }

    [Fact]
    public void GetStopWords_EmptyConfig_ReturnsEmptySet()
    {
        // Arrange
        var options = Options.Create(new FrequencyAnalysisOptions
        {
            StopWords = []
        });
        var provider = new StopWordsProvider(options);

        // Act
        var result = provider.GetStopWords();

        // Assert
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetStopWords_WhitespaceEntries_FiltersOutEmptyStrings()
    {
        // Arrange
        var options = Options.Create(new FrequencyAnalysisOptions
        {
            StopWords = ["  ", "", "the", "   and   "]
        });
        var provider = new StopWordsProvider(options);

        // Act
        var result = provider.GetStopWords();

        // Assert
        result.Should().HaveCount(2);
        result.Should().Contain("the");
        result.Should().Contain("and");
    }

    [Fact]
    public void GetStopWords_CaseInsensitiveLookup_WorksCorrectly()
    {
        // Arrange
        var options = Options.Create(new FrequencyAnalysisOptions
        {
            StopWords = ["the", "and"]
        });
        var provider = new StopWordsProvider(options);

        // Act
        var result = provider.GetStopWords();

        // Assert
        result.Contains("THE").Should().BeTrue();
        result.Contains("The").Should().BeTrue();
        result.Contains("AND").Should().BeTrue();
    }
}
