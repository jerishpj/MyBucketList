using MyBucketList.Api.Shared.Interfaces;

namespace MyBucketList.Api.Features.BucketItem.Queries
{
    public record GetAllBucketItemsQuery : IQuery<List<BucketItemDto>>;

    public record BucketItemDto(
        int Id,
        string Title,
        string? Description,
        bool IsCompleted,
        DateTime CreatedAt,
        DateTime? CompletedAt,
        int Priority
    );
}
