using AceIt.Models;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Data;

public static class DbSeeder
{
    public static async Task Seed(AppDbContext db)
    {
        if (await db.Questions.AnyAsync()) return;

        db.Questions.AddRange(
           new Question { Text = "Explain async/await", Topic = Topic.CSharp }, 
           new Question { Text = "What is dependency injection?", Topic = Topic.DotNet }
        );
        
        await db.SaveChangesAsync();
    }
}