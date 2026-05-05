using Microsoft.EntityFrameworkCore;
using MyBucketList.Api.Infrastructure.Data;

namespace MyBucketList.Api.Test.Helpers
{
    public static class TestDbContextFactory
    {
        public static AppDbContext CreateInMemoryDbContext(string? databaseName = null)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging()
                .Options;

            return new AppDbContext(options);
        }

        public static AppDbContext CreateSeededDbContext(string? databaseName = null)
        {
            var context = CreateInMemoryDbContext(databaseName);

            // Seed test data
            context.BucketItems.AddRange(
                new MyBucketList.Api.Features.BucketItem.Entity.BucketItem
                {
                    Id = 1,
                    Title = "Visit the Grand Canyon",
                    Description = "Experience the breathtaking views",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-10),
                    Priority = 1
                },
                new MyBucketList.Api.Features.BucketItem.Entity.BucketItem
                {
                    Id = 2,
                    Title = "Learn to play guitar",
                    Description = "Take guitar lessons",
                    IsCompleted = true,
                    CreatedAt = DateTime.UtcNow.AddDays(-30),
                    CompletedAt = DateTime.UtcNow.AddDays(-5),
                    Priority = 2
                },
                new MyBucketList.Api.Features.BucketItem.Entity.BucketItem
                {
                    Id = 3,
                    Title = "Run a marathon",
                    Description = "Complete a full 42km marathon",
                    IsCompleted = false,
                    CreatedAt = DateTime.UtcNow.AddDays(-5),
                    Priority = 3
                }
            );

            context.SaveChanges();
            return context;
        }
    }
}
