using AceIt.Exceptions;
using AceIt.Services;

namespace AceIt.Tests;

public class GradingResponseParserTests
{
    private const string ValidXml = """
        <results>
          <result><questionId>1</questionId><score>8</score><feedback>Good.</feedback></result>
          <result><questionId>2</questionId><score>5</score><feedback>Okay.</feedback></result>
        </results>
        """;

    [Fact]
    public void Parse_ValidXml_ReturnsAllResults()
    {
        var results = GradingResponseParser.Parse(ValidXml);

        Assert.Equal(2, results.Count);
        Assert.Equal(1, results[0].QuestionId);
        Assert.Equal(8, results[0].Score);
        Assert.Equal("Good.", results[0].Feedback);
    }

    [Fact]
    public void Parse_WrappedInMarkdownCodeFences_StillParses()
    {
        var fenced = $"```xml\n{ValidXml}\n```";

        var results = GradingResponseParser.Parse(fenced);

        Assert.Equal(2, results.Count);
    }

    [Fact]
    public void Parse_MalformedXml_ThrowsExternalService()
    {
        Assert.Throws<ExternalServiceException>(
            () => GradingResponseParser.Parse("this is not xml <result"));
    }

    [Fact]
    public void Parse_SkipsResultsWithNonNumericScore()
    {
        var xml = """
            <results>
              <result><questionId>1</questionId><score>eight</score><feedback>x</feedback></result>
              <result><questionId>2</questionId><score>6</score><feedback>y</feedback></result>
            </results>
            """;

        var results = GradingResponseParser.Parse(xml);

        Assert.Single(results);
        Assert.Equal(2, results[0].QuestionId);
    }

    [Fact]
    public void Parse_ClampsOutOfRangeScore()
    {
        var xml = """
            <results>
              <result><questionId>1</questionId><score>50</score><feedback>x</feedback></result>
            </results>
            """;

        var results = GradingResponseParser.Parse(xml);

        Assert.Equal(10, results[0].Score);
    }

    [Fact]
    public void Parse_WhenNoValidResults_Throws()
    {
        Assert.Throws<ExternalServiceException>(
            () => GradingResponseParser.Parse("<results></results>"));
    }
}
