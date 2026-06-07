using AceIt.DTOs;

namespace AceIt;

public interface ISessionService
{
    Task<SessionDto> StartSession();
    Task<ResultDto> FinishSession(FinishSessionRequest request);
}