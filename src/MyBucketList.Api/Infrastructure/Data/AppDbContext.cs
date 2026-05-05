using Microsoft.EntityFrameworkCore;
using MyBucketList.Api.Features.BucketItem.Entity;

namespace MyBucketList.Api.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<BucketItem> BucketItems => Set<BucketItem>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BucketItem>(entity =>
            {
                entity.ToTable("BucketItems");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Description)
                    .HasMaxLength(1000);

                entity.Property(e => e.IsCompleted)
                    .IsRequired()
                    .HasDefaultValue(false);

                entity.Property(e => e.CreatedAt)
                    .IsRequired()
                    .HasDefaultValueSql("GETUTCDATE()");

                entity.Property(e => e.CompletedAt)
                    .IsRequired(false);

                entity.Property(e => e.Priority)
                    .IsRequired()
                    .HasDefaultValue(1);

                // Create indexes for better query performance
                entity.HasIndex(e => e.IsCompleted)
                    .HasDatabaseName("IX_BucketItems_IsCompleted");

                entity.HasIndex(e => e.Priority)
                    .HasDatabaseName("IX_BucketItems_Priority");
            });

            // Seed initial data (optional)
            modelBuilder.Entity<BucketItem>().HasData(
                new BucketItem
                {
                    Id = 1,
                    Title = "Visit the Grand Canyon",
                    Description = "Experience the breathtaking views of the Grand Canyon.",
                    IsCompleted = false,
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                    Priority = 1
                },
                new BucketItem
                {
                    Id = 2,
                    Title = "Learn to play the guitar",
                    Description = "Take guitar lessons and learn to play your favorite songs.",
                    IsCompleted = true,
                    CreatedAt = new DateTime(2023, 12, 1, 0, 0, 0, DateTimeKind.Utc),
                    CompletedAt = new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc),
                    Priority = 2
                }
            );
        }

    }
}
