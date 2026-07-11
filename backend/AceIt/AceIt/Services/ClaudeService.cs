using System.Text.Json;
using AceIt.DTOs;
using AceIt.Exceptions;
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
        Message res;
        try
        {
            res = await client.Messages.Create(parameters);
        }
        catch (Exception ex)
        {
            throw new ExternalServiceException("Claude request failed.", ex);
        }

        if (res.Content.Count == 0)
            throw new ExternalServiceException("Claude returned an empty response.");

        res.Content[0].TryPickText(out var textBlock);
        var rawText = textBlock?.Text
            ?? throw new ExternalServiceException("Claude returned no text content.");

        var results = GradingResponseParser.Parse(rawText);
        return new SessionSummaryDto(results);
    }
}
