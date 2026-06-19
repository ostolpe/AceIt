using AceIt.DTOs;

namespace AceIt.Services;

public interface ISessionService
{
    Task<SessionDto> StartSession(string userId);
    Task<SessionSummaryDto> FinishSession(FinishSessionRequest request);
}
