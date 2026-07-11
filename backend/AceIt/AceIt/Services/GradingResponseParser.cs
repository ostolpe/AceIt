using System.Xml;
using System.Xml.Linq;
using AceIt.DTOs;
using AceIt.Exceptions;

namespace AceIt.Services;

internal static class GradingResponseParser
{
    public static List<QuestionResultDto> Parse(string rawText)
    {
        var cleanXml = rawText
            .Replace("```xml", "")
            .Replace("```", "")
            .Trim();

        XDocument document;
        try
        {
            document = XDocument.Parse(cleanXml);
        }
        catch (XmlException ex)
        {
            throw new ExternalServiceException(
                "Claude returned a grading response that was not valid XML.", ex);
        }

        var results = new List<QuestionResultDto>();
        foreach (var element in document.Descendants("result"))
        {
            if (!int.TryParse(element.Element("questionId")?.Value, out var questionId))
                continue;
            if (!int.TryParse(element.Element("score")?.Value, out var score))
                continue;

            var feedback = element.Element("feedback")?.Value.Trim() ?? string.Empty;
            results.Add(new QuestionResultDto(questionId, Math.Clamp(score, 0, 10), feedback));
        }

        if (results.Count == 0)
            throw new ExternalServiceException("Claude returned no gradeable results.");

        return results;
    }
}
