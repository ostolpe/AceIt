using AceIt.DTOs;

namespace AceIt.Services;

public class ClaudeService : IAiService
{
    public async Task<ResultDto> GradeSession(FinishSessionRequest request)
    {
        //TODO: implement grading from claude
        return new ResultDto("GREAT JOB, PERFECT");
    }
}