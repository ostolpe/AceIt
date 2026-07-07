using System.Text.Json;
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

        if (res.Content.Count == 0)
            throw new InvalidOperationException("Claude returned an empty response.");

        res.Content[0].TryPickText(out var textBlock);
        var rawText = textBlock?.Text
            ?? throw new InvalidOperationException("Claude returned no text content.");

        var results = GradingResponseParser.Parse(rawText);
        return new SessionSummaryDto(results);
    }
}
