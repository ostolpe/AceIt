using AceIt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User, IdentityRole, string>(options)
{
    public DbSet<Question> Questions { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<QuestionResult> QuestionResults { get; set; }
}