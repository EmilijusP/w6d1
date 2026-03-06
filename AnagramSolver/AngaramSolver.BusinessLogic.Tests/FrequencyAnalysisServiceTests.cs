using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;

namespace AnagramSolver.BusinessLogic.Tests;

public class FrequencyAnalysisServiceTests
{
    private readonly Mock<IStopWordsProvider> _mockStopWordsProvider;
    private readonly FrequencyAnalysisOptions _options;
    private readonly FrequencyAnalysisService _frequencyAnalysisService;

    public FrequencyAnalysisServiceTests()
    {
        _mockStopWordsProvider = new Mock<IStopWordsProvider>();
        _mockStopWordsProvider.Setup(x => x.GetStopWords())
            .Returns(new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "the", "a", "an", "is", "are" });

        _options = new FrequencyAnalysisOptions
        {
            MaxTextLength = 100000,
            TopWordsCount = 10,
            StopWords = ["the", "a", "an", "is", "are"]
        };

        var optionsMock = Options.Create(_options);

            _frequencyAnalysisService = new FrequencyAnalysisService(_mockStopWordsProvider.Object, optionsMock);
        }

    [Fact]
    public async Task AnalyzeAsync_ValidText_ReturnsTop10AndCounts()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "apple banana apple cherry banana apple dog elephant frog grape honey ice jam kite lemon mango"
        };

        // Act
        var result = await _frequencyAnalysisService.AnalyzeAsync(request);

        // Assert
        result.TotalWords.Should().Be(16);
        result.UniqueWords.Should().Be(13);
        result.TopWords.Should().HaveCountLessThanOrEqualTo(10);
        result.TopWords[0].Word.Should().Be("apple");
        result.TopWords[0].Count.Should().Be(3);
        result.LongestWord.Should().Be("elephant");
    }

    [Fact]
    public async Task AnalyzeAsync_EmptyText_ThrowsValidationException()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest { Text = "" };

        // Act
        Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("request")
            .WithMessage("*empty*whitespace*");
    }

    [Fact]
    public async Task AnalyzeAsync_WhitespaceOnlyText_ThrowsValidationException()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest { Text = "   \t\n  " };

        // Act
        Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("request")
            .WithMessage("*empty*whitespace*");
    }

    [Fact]
    public async Task AnalyzeAsync_SpecialCharactersOnly_ThrowsValidationException()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest { Text = "!!!@@@###$$$%%%^^^" };

        // Act
        Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithParameterName("request")
            .WithMessage("*no valid words*");
    }

    [Fact]
    public async Task AnalyzeAsync_CaseVariants_AggregatesCaseInsensitive()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "Apple APPLE apple ApPlE"
            };

            // Act
            var result = await _frequencyAnalysisService.AnalyzeAsync(request);

            // Assert
            result.UniqueWords.Should().Be(1);
        result.TotalWords.Should().Be(4);
        result.TopWords.Should().ContainSingle();
        result.TopWords[0].Count.Should().Be(4);
    }

    [Fact]
    public async Task AnalyzeAsync_WithStopWords_ExcludesStopWords()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "The apple is a fruit and the banana is also a fruit"
            };

            // Act
            var result = await _frequencyAnalysisService.AnalyzeAsync(request);

            // Assert
            result.TopWords.Should().NotContain(w => w.Word.Equals("the", StringComparison.OrdinalIgnoreCase));
        result.TopWords.Should().NotContain(w => w.Word.Equals("is", StringComparison.OrdinalIgnoreCase));
        result.TopWords.Should().NotContain(w => w.Word.Equals("a", StringComparison.OrdinalIgnoreCase));
        result.TopWords.Should().Contain(w => w.Word.Equals("fruit", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task AnalyzeAsync_TieFrequencies_ReturnsDeterministicOrder()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "zebra apple mango cherry"
                };

                // Act
                var result = await _frequencyAnalysisService.AnalyzeAsync(request);

                // Assert
                result.TopWords.Select(w => w.Word).Should().BeInAscendingOrder(StringComparer.OrdinalIgnoreCase);
            }

    [Fact]
    public async Task AnalyzeAsync_MultipleLongestWords_ReturnsLexicographicallySmallest()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "elephant dinosaur"
                };

                // Act
                var result = await _frequencyAnalysisService.AnalyzeAsync(request);

                // Assert
                result.LongestWord.Should().Be("dinosaur");
            }

    [Fact]
    public async Task AnalyzeAsync_LithuanianDiacritics_ProcessesCorrectly()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "?žuolas ?žuolas šermukšnis šermukšnis šermukšnis"
            };

            // Act
            var result = await _frequencyAnalysisService.AnalyzeAsync(request);

            // Assert
            result.TotalWords.Should().Be(5);
        result.UniqueWords.Should().Be(2);
        result.TopWords[0].Word.Should().Be("šermukšnis");
        result.TopWords[0].Count.Should().Be(3);
        result.LongestWord.Should().Be("šermukšnis");
    }

    [Fact]
    public async Task AnalyzeAsync_NewlinesAndTabs_ParsesCorrectly()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "apple\nbanana\tcherry\r\napple"
            };

            // Act
            var result = await _frequencyAnalysisService.AnalyzeAsync(request);

            // Assert
            result.TotalWords.Should().Be(4);
        result.UniqueWords.Should().Be(3);
        result.TopWords[0].Word.Should().Be("apple");
        result.TopWords[0].Count.Should().Be(2);
    }

    [Fact]
    public async Task AnalyzeAsync_MoreThanTenWords_ReturnsOnlyTopTen()
    {
        // Arrange
        var words = new[] { "apple", "banana", "cherry", "date", "elderberry", "fig", "grape", "honeydew", "imbe", "jackfruit", "kiwi", "lemon", "mango", "nectarine", "orange" };
        var request = new FrequencyAnalysisRequest
        {
            Text = string.Join(" ", words)
                };

                // Act
                var result = await _frequencyAnalysisService.AnalyzeAsync(request);

                // Assert
                result.TopWords.Should().HaveCount(10);
            }

    [Fact]
    public async Task AnalyzeAsync_CancellationRequested_ThrowsOperationCanceledException()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest { Text = "apple banana cherry" };
        var cts = new CancellationTokenSource();
        cts.Cancel();

        // Act
        Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request, cts.Token);

        // Assert
        await act.Should().ThrowAsync<OperationCanceledException>();
    }

    [Fact]
    public async Task AnalyzeAsync_TextExceedsMaxLength_ThrowsValidationException()
    {
        // Arrange
        var shortMaxOptions = new FrequencyAnalysisOptions
        {
            MaxTextLength = 10,
            TopWordsCount = 10,
            StopWords = []
        };
        var service = new FrequencyAnalysisService(
            _mockStopWordsProvider.Object, 
            Options.Create(shortMaxOptions));
        
        var request = new FrequencyAnalysisRequest
        {
            Text = "This text is definitely longer than 10 characters"
        };

        // Act
        Func<Task> act = async () => await service.AnalyzeAsync(request);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("*exceeds*maximum*");
    }

    [Fact]
    public async Task AnalyzeAsync_WordsWithApostrophes_ProcessesCorrectly()
    {
        // Arrange
        var request = new FrequencyAnalysisRequest
        {
            Text = "don't won't can't it's"
            };

            // Act
            var result = await _frequencyAnalysisService.AnalyzeAsync(request);

            // Assert
            result.TotalWords.Should().Be(4);
        result.UniqueWords.Should().Be(4);
    }

    [Fact]
    public async Task AnalyzeAsync_WordsWithHyphens_ProcessesCorrectly()
    {
        // Arrange
                var request = new FrequencyAnalysisRequest
                {
                    Text = "well-known self-aware mother-in-law"
                    };

                    // Act
                    var result = await _frequencyAnalysisService.AnalyzeAsync(request);

                    // Assert
                    result.TotalWords.Should().Be(3);
                result.UniqueWords.Should().Be(3);
            }

            [Fact]
            public async Task AnalyzeAsync_NullRequest_ThrowsArgumentNullException()
            {
                // Arrange
                FrequencyAnalysisRequest? request = null;

                // Act
                Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request!);

                // Assert
                await act.Should().ThrowAsync<ArgumentNullException>()
                    .WithParameterName("request");
            }

            [Fact]
            public async Task AnalyzeAsync_OnlyStopWords_ThrowsArgumentException()
            {
                // Arrange
                var request = new FrequencyAnalysisRequest
                {
                    Text = "the a an is are the the a"
                };

                // Act
                Func<Task> act = async () => await _frequencyAnalysisService.AnalyzeAsync(request);

                // Assert
                await act.Should().ThrowAsync<ArgumentException>()
                    .WithMessage("*no valid words*");
            }

            [Fact]
            public async Task AnalyzeAsync_MixedCaseStopWords_ExcludesAll()
            {
                // Arrange
                var request = new FrequencyAnalysisRequest
                {
                    Text = "THE Apple Is A fruit AN orange ARE delicious"
                };

                // Act
                var result = await _frequencyAnalysisService.AnalyzeAsync(request);

                // Assert
                result.TopWords.Should().NotContain(w => 
                    w.Word.Equals("the", StringComparison.OrdinalIgnoreCase) ||
                    w.Word.Equals("is", StringComparison.OrdinalIgnoreCase) ||
                    w.Word.Equals("a", StringComparison.OrdinalIgnoreCase) ||
                    w.Word.Equals("an", StringComparison.OrdinalIgnoreCase) ||
                    w.Word.Equals("are", StringComparison.OrdinalIgnoreCase));
                result.UniqueWords.Should().Be(4);
            }

            [Fact]
            public void Constructor_NullStopWordsProvider_ThrowsArgumentNullException()
            {
                // Arrange
                IStopWordsProvider? nullProvider = null;
                var options = Options.Create(_options);

                // Act
                Action act = () => new FrequencyAnalysisService(nullProvider!, options);

                // Assert
                act.Should().Throw<ArgumentNullException>()
                    .WithParameterName("stopWordsProvider");
            }

            [Fact]
            public void Constructor_NullOptions_ThrowsArgumentNullException()
            {
                // Arrange
                IOptions<FrequencyAnalysisOptions>? nullOptions = null;

                // Act
                Action act = () => new FrequencyAnalysisService(_mockStopWordsProvider.Object, nullOptions!);

                // Assert
                act.Should().Throw<ArgumentNullException>();
            }
        }
