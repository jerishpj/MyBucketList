using Microsoft.AspNetCore.Mvc;
using MyBucketList.Api.Features.BucketItem.Commands;
using MyBucketList.Api.Features.BucketItem.Queries;
using MyBucketList.Api.Infrastructure.Data;

namespace MyBucketList.Api.Features.BucketItem
{
    [ApiController]
    [Route("[controller]")]
    public class BucketItemController : ControllerBase
    {
        private readonly ILogger<BucketItemController> _logger;
        private readonly AppDbContext _dbContext;
        private readonly GetAllBucketItemsQueryHandler _getAllBucketItemsQueryHandler;
        private readonly BucketItemCreateCommandHandler _createCommandHandler;

        public BucketItemController(
            ILogger<BucketItemController> logger,
            AppDbContext dbContext,
            GetAllBucketItemsQueryHandler getAllBucketItemsQueryHandler,
            BucketItemCreateCommandHandler createCommandHandler)
        {
            _logger = logger;
            _dbContext = dbContext;
            _getAllBucketItemsQueryHandler = getAllBucketItemsQueryHandler;
            _createCommandHandler = createCommandHandler;
        }

        // GET: api/BucketItem
        [HttpGet(Name = "GetAllBucketItems")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Fetching all bucket items");

                var query = new GetAllBucketItemsQuery();
                var bucketItems = await _getAllBucketItemsQueryHandler.Handle(query, cancellationToken);

                return Ok(bucketItems);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching bucket items");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request");
            }
        }

        // POST: api/BucketItem
        [HttpPost(Name = "CreateBucketItem")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create(
            [FromBody] CreateBucketItemCommand command,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating new bucket item: {Title}", command.Title);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var response = await _createCommandHandler.Handle(command, cancellationToken);

                return CreatedAtRoute(
                    "GetBucketItemById",
                    new { id = response.Id },
                    response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating bucket item");
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "An error occurred while processing your request");
            }
        }

    }
}
