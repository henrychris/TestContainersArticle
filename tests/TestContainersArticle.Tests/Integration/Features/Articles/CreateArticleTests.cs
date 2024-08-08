using FluentAssertions;
using Microsoft.Extensions.Logging;
using TestContainersArticle.Main.Features.CreateArticle;
using TestContainersArticle.Tests.Base;

namespace TestContainersArticle.Tests.Integration.Features.Articles
{
    [TestFixture]
    internal class CreateArticleTests : TestBase
    {
        private ILogger<CreateArticleHandler> _logger = null!;
        private CreateArticleValidator _validator = null!;
        private CreateArticleHandler _handler = null!;

        [SetUp]
        public new void Setup()
        {
            _logger = LoggerFactory.Create(x => x.AddConsole()).CreateLogger<CreateArticleHandler>();
            _validator = new CreateArticleValidator();

            _handler = new CreateArticleHandler(unitOfWork, _validator, _logger);
        }

        [Test]
        public async Task ValidRequest_ReturnsSuccess()
        {
            // arrange
            var request = new CreateArticleRequest
            {
                Title = "Title",
                Content = "content",
                DateToPublish = DateTime.UtcNow
            };

            // act
            var res = await _handler.Handle(request, CancellationToken.None);

            // assert
            res.IsSuccess.Should().BeTrue();
        }
    }
}
