using Microsoft.EntityFrameworkCore;
using MyBucketList.Api.Infrastructure.Data;

namespace MyBucketList.Api.Features.BucketItem.Queries
{
    public class GetAllBucketItemsQueryHandler
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<GetAllBucketItemsQueryHandler> _logger;

        public GetAllBucketItemsQueryHandler(AppDbContext dbContext,
                        ILogger<GetAllBucketItemsQueryHandler> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<BucketItemDto>> Handle(
        GetAllBucketItemsQuery query,
        CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Retrieving all bucket items from database");

            var items = await _dbContext.BucketItems
                .AsNoTracking()
                .OrderByDescending(x => x.Priority)
                .ThenByDescending(x => x.CreatedAt)
                .Select(x => new BucketItemDto(
                    x.Id,
                    x.Title,
                    x.Description,
                    x.IsCompleted,
                    x.CreatedAt,
                    x.CompletedAt,
                    x.Priority
                ))
                .ToListAsync(cancellationToken);

            _logger.LogInformation("Retrieved {Count} bucket items from database", items.Count);

            return items;
        }
    }
}
