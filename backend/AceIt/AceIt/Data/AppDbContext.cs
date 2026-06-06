using AceIt.Models;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<AiResponse> AiResponses { get; set; }
}