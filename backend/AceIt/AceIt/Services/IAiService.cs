using AceIt.DTOs;

namespace AceIt.Services;

public interface IAiService
{
    Task<ResultDto> GradeSession(FinishSessionRequest request);
}