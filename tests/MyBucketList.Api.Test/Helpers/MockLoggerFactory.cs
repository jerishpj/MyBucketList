using Microsoft.Extensions.Logging;
using Moq;

namespace MyBucketList.Api.Test.Helpers
{
    public static class MockLoggerFactory
    {
        public static ILogger<T> CreateLogger<T>()
        {
            return new Mock<ILogger<T>>().Object;
        }

        public static Mock<ILogger<T>> CreateMockLogger<T>()
        {
            return new Mock<ILogger<T>>();
        }
    }
}
