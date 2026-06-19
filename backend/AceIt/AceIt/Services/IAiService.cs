using AceIt.DTOs;

namespace AceIt.Services;

public interface IAiService
{
    Task<SessionSummaryDto> GradeSession(FinishSessionRequest request);
}
