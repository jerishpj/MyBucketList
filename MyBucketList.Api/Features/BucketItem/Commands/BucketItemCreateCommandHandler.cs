using MyBucketList.Api.Infrastructure.Data;

namespace MyBucketList.Api.Features.BucketItem.Commands
{
    public class BucketItemCreateCommandHandler
    {
        private readonly ILogger<BucketItemCreateCommandHandler> _logger;

        private readonly AppDbContext _dbContext;

        public BucketItemCreateCommandHandler(AppDbContext dbContext, ILogger<BucketItemCreateCommandHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<CreateBucketItemResponse> Handle(
            CreateBucketItemCommand command,
            CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating new bucket item with title: {Title}", command.Title);

            // Create the entity from command
            var bucketItem = new Entity.BucketItem
            {
                Title = command.Title,
                Description = command.Description,
                Priority = command.Priority,
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };

            // Add to DbContext
            _dbContext.BucketItems.Add(bucketItem);

            // Save changes to database
            await _dbContext.SaveChangesAsync(cancellationToken);

            _logger.LogInformation("Bucket item created successfully with ID: {Id}", bucketItem.Id);

            // Map entity to response
            return new CreateBucketItemResponse(
                bucketItem.Id,
                bucketItem.Title,
                bucketItem.Description,
                bucketItem.CreatedAt,
                bucketItem.Priority
            );
        }

    }
}
