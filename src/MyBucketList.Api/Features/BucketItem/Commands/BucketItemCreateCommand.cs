using MyBucketList.Api.Shared.Interfaces;

namespace MyBucketList.Api.Features.BucketItem.Commands
{
    public record CreateBucketItemCommand(
    string Title,
    string? Description,
    int Priority = 1
) : ICommand<CreateBucketItemResponse>;

    public record CreateBucketItemResponse(
        int Id,
        string Title,
        string? Description,
        DateTime CreatedAt,
        int Priority
    );
}
