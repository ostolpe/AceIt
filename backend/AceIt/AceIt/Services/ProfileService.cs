using AceIt.Data;
using AceIt.DTOs;
using AceIt.Exceptions;
using AceIt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Services;

public class ProfileService(AppDbContext db, UserManager<User> userManager) : IProfileService
{
    public async Task<ProfileDto> GetProfileDataAsync(string userId)
    {
        var user = await userManager.FindByIdAsync(userId)
        ?? throw new NotFoundException("User not found");

        var userSessions = await db.Sessions
              .Where(s => s.UserId == userId)
              .Include(s => s.Results)
              .ThenInclude(r => r.Question)
              .AsNoTracking()
              .ToListAsync();

        var userResults = userSessions
            .SelectMany(s => s.Results)
            .ToList();

        var dto = new ProfileDto(
            Email: user.Email!,
            TotalSessions: userSessions.Count,
            TotalQuestionsAnswered: userResults.Count,
            OverallAverage: userResults.Any() ? userResults.Average(r => r.Score) : 0,
            TopicStats: userResults
                .GroupBy(r => r.Question.Topic)
                .Select(g => new TopicStat(g.Key.ToString(), g.Average(r => r.Score), g.Count()))
                .ToList()
        );
        return dto;
    }
}
