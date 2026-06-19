using AceIt.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AceIt.Data;

public static class DbSeeder
{
    public static async Task Seed(AppDbContext db, UserManager<User> userManager)
    {
        if (!await db.Questions.AnyAsync())
        {
            db.Questions.AddRange(
                // C# Language
                new Question { Text = "Explain async/await and when you would use it.", Topic = Topic.CSharp },
                new Question { Text = "What is the difference between `ref` and `out` parameters?", Topic = Topic.CSharp },
                new Question { Text = "What is the difference between `const` and `readonly`?", Topic = Topic.CSharp },
                new Question { Text = "What are nullable value types and how do you declare one?", Topic = Topic.CSharp },

                // .NET / ASP.NET Core
                new Question { Text = "What is dependency injection and why is it useful?", Topic = Topic.DotNet },
                new Question { Text = "What is middleware in ASP.NET Core and how does the pipeline work?", Topic = Topic.DotNet },
                new Question { Text = "What is the difference between Transient, Scoped, and Singleton service lifetimes?", Topic = Topic.DotNet },

                // OOP
                new Question { Text = "What are the four pillars of object-oriented programming?", Topic = Topic.OOP },
                new Question { Text = "What is the difference between an abstract class and an interface in C#?", Topic = Topic.OOP },
                new Question { Text = "What is the difference between method overriding and method overloading?", Topic = Topic.OOP },
                new Question { Text = "What does the `sealed` keyword do to a class?", Topic = Topic.OOP },

                // Collections
                new Question { Text = "What is the difference between `List<T>` and an array in C#?", Topic = Topic.Collections },
                new Question { Text = "When would you use a `Dictionary<TKey, TValue>` over a `List<T>`?", Topic = Topic.Collections },
                new Question { Text = "What is `IEnumerable<T>` and why is it useful?", Topic = Topic.Collections },

                // LINQ
                new Question { Text = "What does LINQ stand for and what problem does it solve?", Topic = Topic.Linq },
                new Question { Text = "What is the difference between `Where`, `Select`, and `FirstOrDefault` in LINQ?", Topic = Topic.Linq },
                new Question { Text = "What is deferred execution in LINQ and why does it matter?", Topic = Topic.Linq },

                // Error Handling
                new Question { Text = "What is the difference between `throw` and `throw ex` when re-throwing an exception?", Topic = Topic.ErrorHandling },
                new Question { Text = "What is the purpose of the `finally` block in a try-catch statement?", Topic = Topic.ErrorHandling },
                new Question { Text = "When should you create a custom exception class instead of using a built-in one?", Topic = Topic.ErrorHandling },

                // Testing
                new Question { Text = "What is the difference between a unit test and an integration test?", Topic = Topic.Testing },
                new Question { Text = "What does the Arrange-Act-Assert (AAA) pattern mean in unit testing?", Topic = Topic.Testing },
                new Question { Text = "What is mocking and why is it useful in unit tests?", Topic = Topic.Testing }
            );

            await db.SaveChangesAsync();
        }

        await SeedTestUser(db, userManager);
    }

    private static async Task SeedTestUser(AppDbContext db, UserManager<User> userManager)
    {
        const string email = "test@aceit.dev";
        if (await userManager.FindByEmailAsync(email) != null) return;

        var user = new User { UserName = email, Email = email, EmailConfirmed = true };
        var result = await userManager.CreateAsync(user, "Test1234!");
        if (!result.Succeeded) return;

        var questions = await db.Questions.ToListAsync();

        // Session 1 — mostly correct, 3 weeks ago
        var session1Questions = questions.Where(q => q.Topic == Topic.CSharp || q.Topic == Topic.OOP).Take(5).ToList();
        var session1 = new Session
        {
            UserId = user.Id,
            Questions = session1Questions,
            CreatedAt = DateTime.UtcNow.AddDays(-21).AddMinutes(-30),
            CompletedAt = DateTime.UtcNow.AddDays(-21),
            Results =
            [
                new QuestionResult { QuestionId = session1Questions[0].Id, UserAnswer = "Async/await is used to write non-blocking code. You use it when calling I/O operations like database queries or HTTP requests so the thread isn't blocked while waiting.", Score = 9, Feedback = "Great explanation of async/await with a clear real-world use case." },
                new QuestionResult { QuestionId = session1Questions[1].Id, UserAnswer = "ref passes a reference to an existing variable, out is used to return a value from a method. Both pass by reference.", Score = 7, Feedback = "Covered the key difference but missed mentioning that `out` doesn't need to be initialised before the call." },
                new QuestionResult { QuestionId = session1Questions[2].Id, UserAnswer = "const is a compile-time constant and can't be changed. readonly can only be set at declaration or in the constructor.", Score = 8, Feedback = "Correct. Could have mentioned that `readonly` can be set in a constructor." },
                new QuestionResult { QuestionId = session1Questions[3].Id, UserAnswer = "Encapsulation, Abstraction, Inheritance, and Polymorphism. Encapsulation hides internal state, abstraction hides complexity, inheritance allows code reuse, polymorphism allows different types to be treated uniformly.", Score = 10, Feedback = "Perfect answer covering all four pillars with concise examples." },
                new QuestionResult { QuestionId = session1Questions[4].Id, UserAnswer = "Overriding is when a subclass provides a new implementation of a base class method. Overloading is defining multiple methods with the same name but different signatures.", Score = 6, Feedback = "Got the main idea but conflated method hiding with overriding." },
            ]
        };

        // Session 2 — mixed results, 1 week ago
        var session2Questions = questions.Where(q => q.Topic == Topic.DotNet || q.Topic == Topic.Collections).Take(5).ToList();
        var session2 = new Session
        {
            UserId = user.Id,
            Questions = session2Questions,
            CreatedAt = DateTime.UtcNow.AddDays(-7).AddMinutes(-25),
            CompletedAt = DateTime.UtcNow.AddDays(-7),
            Results =
            [
                new QuestionResult { QuestionId = session2Questions[0].Id, UserAnswer = "Dependency injection is when you pass dependencies into a class instead of creating them inside it. It makes code more modular.", Score = 5, Feedback = "Defined DI correctly but couldn't explain the benefits in a testability context." },
                new QuestionResult { QuestionId = session2Questions[1].Id, UserAnswer = "Middleware are components in the request pipeline. Each one can inspect or modify the request and response, then either pass it to the next middleware or short-circuit.", Score = 8, Feedback = "Good overview of the middleware pipeline and request/response flow." },
                new QuestionResult { QuestionId = session2Questions[2].Id, UserAnswer = "Transient creates a new instance every time. Scoped creates one per HTTP request. Singleton creates one for the entire app lifetime.", Score = 9, Feedback = "Clear distinction between all three lifetimes with good examples." },
                new QuestionResult { QuestionId = session2Questions[3].Id, UserAnswer = "List<T> is dynamic and can grow, arrays are fixed size. Lists are more flexible.", Score = 4, Feedback = "Mentioned fixed vs dynamic size but missed performance implications." },
                new QuestionResult { QuestionId = session2Questions[4].Id, UserAnswer = "Dictionary is better when you need to look things up by key. List is better when you just need an ordered collection to iterate.", Score = 7, Feedback = "Correct use-case for Dictionary, but didn't mention O(1) lookup complexity." },
            ]
        };

        // Session 3 — recent, strong performance, yesterday
        var session3Questions = questions.Where(q => q.Topic == Topic.Linq || q.Topic == Topic.Testing).Take(5).ToList();
        var session3 = new Session
        {
            UserId = user.Id,
            Questions = session3Questions,
            CreatedAt = DateTime.UtcNow.AddDays(-1).AddMinutes(-20),
            CompletedAt = DateTime.UtcNow.AddDays(-1),
            Results =
            [
                new QuestionResult { QuestionId = session3Questions[0].Id, UserAnswer = "LINQ stands for Language Integrated Query. It lets you query collections using a consistent syntax, instead of writing manual loops for filtering, sorting, and transforming data.", Score = 10, Feedback = "Excellent — named it correctly and explained the problem it solves well." },
                new QuestionResult { QuestionId = session3Questions[1].Id, UserAnswer = "Where filters a collection, Select projects it to a new shape, FirstOrDefault returns the first match or null if nothing is found.", Score = 9, Feedback = "Solid answer on all three operators. Minor: `FirstOrDefault` returns null not default(T) for reference types." },
                new QuestionResult { QuestionId = session3Questions[2].Id, UserAnswer = "Deferred execution means the LINQ query isn't run when you define it, but when you enumerate it. This means you can build up queries without hitting the database until you actually need the results.", Score = 8, Feedback = "Good grasp of deferred execution, could have shown a concrete pitfall." },
                new QuestionResult { QuestionId = session3Questions[3].Id, UserAnswer = "A unit test tests a single piece of logic in isolation, usually mocking dependencies. An integration test tests how multiple components work together, often with a real database or service.", Score = 10, Feedback = "Spot on. Clear boundary between unit and integration tests." },
                new QuestionResult { QuestionId = session3Questions[4].Id, UserAnswer = "Arrange sets up the test data, Act calls the method you're testing, Assert verifies the result. It keeps tests readable and structured.", Score = 9, Feedback = "Great explanation of AAA with a practical example." },
            ]
        };

        db.Sessions.AddRange(session1, session2, session3);
        await db.SaveChangesAsync();
    }
}
