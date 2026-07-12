using AceIt.DTOs;

namespace AceIt.Services;

public interface ISessionService
{
    Task<SessionDto> StartSession(string userId, CancellationToken cancellationToken = default);
    Task<SessionSummaryDto> FinishSession(string userId, FinishSessionRequest request, CancellationToken cancellationToken = default);
}
