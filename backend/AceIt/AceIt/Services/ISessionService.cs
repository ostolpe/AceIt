using AceIt.DTOs;

namespace AceIt.Services;

public interface ISessionService
{
    Task<SessionDto> StartSession();
    Task<ResultDto> FinishSession(FinishSessionRequest request);
}
