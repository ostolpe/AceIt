using System.Text.Json;
using AceIt.DTOs;
using Anthropic;
using Anthropic.Models.Messages;

namespace AceIt.Services;

public class ClaudeService(IConfiguration config) : IAiService
{
    public async Task<ResultDto> GradeSession(FinishSessionRequest request)
    {
        var client = new AnthropicClient
            { ApiKey = config["Anthropic:ANTHROPIC_API_KEY"] };

        var parameters = new MessageCreateParams
        {
            MaxTokens = 1024,
            System =
                "You are an expert interviewer grading junior .NET developer answers. For each question and answer provided, give specific feedback on what was correct, what was missing, and a score from 1-10. Be honest and direct.",
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
        res.Content[0].TryPickText(out var text);
        return new ResultDto(text?.Text ?? "No Response");
    }
}