using AceIt.DTOs;

namespace AceIt.Services;

public interface IProfileService
{
    public Task<ProfileDto> GetProfileDataAsync(string userId);
}
