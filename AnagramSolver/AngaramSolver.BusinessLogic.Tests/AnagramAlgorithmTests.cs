using AnagramSolver.BusinessLogic.Services;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using FluentAssertions;

namespace AnagramSolver.BusinessLogic.Tests;

public class AnagramAlgorithmTests
{
    private readonly IComplexAnagramAlgorithm _anagramAlgorithm;

    public AnagramAlgorithmTests()
    {
        _anagramAlgorithm = new ComplexAnagramAlgorithm();
    }

    public static IEnumerable<object[]> GetFindKeyCombinationsTestData()
    {
        var targetLetters = new Dictionary<char, int>
        {
            ['t'] = 2,
            ['e'] = 1,
            ['s'] = 1
        };

        var possibleAnagrams = new List<Anagram>
        {
            new Anagram { Key = "estt", KeyCharCount = new Dictionary<char, int> {['e']=1, ['s']=1, ['t']=2} },
            new Anagram { Key = "est", KeyCharCount = new Dictionary<char, int> {['e']=1, ['s']=1, ['t']=1} },
            new Anagram { Key = "es", KeyCharCount = new Dictionary<char, int> {['e']=1, ['s']=1} },
            new Anagram { Key = "tt", KeyCharCount = new Dictionary<char, int> {['t']=2} },
            new Anagram { Key = "t", KeyCharCount = new Dictionary<char, int> {['t']=1} }
        };

        yield return new object[] 
        { 
            targetLetters,
            1,
            possibleAnagrams,
            new List<List<string>> 
            { 
                new List<string> { "estt" } 
            } 
        };
        yield return new object[] 
        { 
            targetLetters,
            2,
            possibleAnagrams,
            new List<List<string>> 
            { 
                new List<string> { "estt" }, 
                new List<string> { "est",  "t" },
                new List<string> { "es", "tt" } 
            } 
        };
        yield return new object[] 
        { 
            targetLetters,
            0,
            possibleAnagrams,
            new List<List<string>> 
            { 
                new List<string> { } 
            } 
        };
    }

    [Theory]
    [MemberData(nameof(GetFindKeyCombinationsTestData))]
    public void FindKeyCombinations_VariousInputs_ReturnsExpectedResult(Dictionary<char, int> targetLetters, int maxWords, List<Anagram> possibleAnagrams, List<List<string>> expectedResult)
    {
        //arrange

        //act
        var result = _anagramAlgorithm.FindKeyCombinations(targetLetters, maxWords, possibleAnagrams);

        //assert
        result.Should().BeEquivalentTo(expectedResult);
    }

    public static IEnumerable<object[]> GetCanFitWithinTestData()
    {
        yield return new object[]
        {
            new Dictionary<char, int>
            {
                ['e'] = 1,
                ['s'] = 1,
                ['t'] = 2
            },

            new Dictionary<char, int>
            {
                ['e'] = 1,
                ['s'] = 1,
                ['t'] = 2
            },

            true
        };

        yield return new object[]
        {
            new Dictionary<char, int>
            {
                ['t'] = 2
            },

            new Dictionary<char, int>
            {
                ['e'] = 1,
                ['s'] = 1,
                ['t'] = 2
            },

            true
        };

        yield return new object[]
        {
            new Dictionary<char, int>
            {
                ['t'] = 3
            },

            new Dictionary<char, int>
            {
                ['e'] = 1,
                ['s'] = 1,
                ['t'] = 2
            },

            false
        };
    }

    public static IEnumerable<object[]> GetCreateCombinationsTestData()
    {
        var possibleAnagrams = new List<Anagram>
        {
            new Anagram { Key = "estt", Words = new List<string> { "sett" } },
            new Anagram { Key = "est", Words = new List<string> { "set", "tes" }},
            new Anagram { Key = "et", Words = new List<string> { "te" }},
            new Anagram { Key = "st", Words = new List<string> { "st" }},
            new Anagram { Key = "t", Words = new List<string> { "t" }}
        };

        yield return new object[]
        {
            new List<List<string>>
            {
                new List<string> { "estt" },
                new List<string> { "est", "t" },
                new List<string> { "et", "st" }
            },

            possibleAnagrams,

            new List<string>
            {
                "sett",
                "set t",
                "tes t",
                "te st"
            }
        };

        yield return new object[]
        {
            new List<List<string>> { },

            possibleAnagrams,

            new List<string> { }
        };
    }

    [Theory]
    [MemberData(nameof(GetCreateCombinationsTestData))]
    public void CreateCombinations_VariousInputs_ReturnsExpectedResult(List<List<string>> keyCombinations, List<Anagram> possibleAnagrams, List<string> expectedResult)
    {
        //arrange

        //act
        var result = _anagramAlgorithm.CreateCombinations(keyCombinations, possibleAnagrams);

        //assert
        result.Should().BeEquivalentTo(expectedResult);
    }
}
