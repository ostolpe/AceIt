using System.Text.Json;
using System.Xml.Linq;
using AceIt.DTOs;
using Anthropic;
using Anthropic.Models.Messages;

namespace AceIt.Services;

public class ClaudeService(IConfiguration config) : IAiService
{
    public async Task<SessionSummaryDto> GradeSession(FinishSessionRequest request)
    {
        var client = new AnthropicClient
        { ApiKey = config["Anthropic:ANTHROPIC_API_KEY"] };
        var systemPrompt = config["Prompts:GradingSystem"]
            ?? throw new InvalidOperationException("Prompts:GradingSystem is not configured.");

        var parameters = new MessageCreateParams
        {
            MaxTokens = 1024,
            System = systemPrompt,
            Messages =
            [
                new MessageParam
                {
                    Role = Role.User,
                    Content = JsonSerializer.Serialize(request)
                }
            ],
            Model = Model.ClaudeHaiku4_5
        };
        var res = await client.Messages.Create(parameters);
        res.Content[0].TryPickText(out var textBlock);
        Console.WriteLine($"Content count: {res.Content.Count}");
        Console.WriteLine($"Raw response: {textBlock?.Text}");
        var rawText = textBlock?.Text ?? throw new InvalidOperationException("No response from Claude.");
        var cleanXml = rawText
            .Replace("```xml", "")
            .Replace("```", "")
            .Trim();
        var xml = XDocument.Parse(cleanXml);

        var results = xml.Descendants("result").Select(r => new QuestionResultDto(
            int.Parse(r.Element("questionId")!.Value),
            int.Parse(r.Element("score")!.Value),
            r.Element("feedback")!.Value
        )).ToList();

        return new SessionSummaryDto(results);
    }
}
