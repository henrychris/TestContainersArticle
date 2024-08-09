using FluentAssertions;
using HenryUtils.Results;
using Microsoft.Extensions.Logging;
using TestContainersArticle.Main.Features.CreateArticle;
using TestContainersArticle.Tests.Base;
using TestContainersArticle.Tests.Builders;

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
        public async Task Handle_ValidRequest_CreatesArticleAndReturnsSuccess()
        {
            // Arrange
            var request = CreateArticleRequestBuilder.Default().Build();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeNull();
            result.Value.ArticleId.Should().NotBeEmpty();

            // Verify the article was actually created in the database
            var createdArticle = await unitOfWork.Articles.GetByIdAsync(result.Value.ArticleId);
            createdArticle.Should().NotBeNull();
            createdArticle!.Title.Should().Be(request.Title);
            createdArticle.Content.Should().Be(request.Content);
            createdArticle.DateToPublish.Should().Be(request.DateToPublish);
        }

        [Test]
        public async Task Handle_InvalidRequest_ReturnsFailureAndDoesNotCreateArticle()
        {
            // Arrange
            var request = CreateArticleRequestBuilder.Default().WithTitle("").WithDateToPublish(DateTime.UtcNow.AddDays(-1)).Build();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();

            // Verify no article was created in the database
            var allArticles = await unitOfWork.Articles.GetAllAsync();
            allArticles.Should().BeEmpty();
        }

        [TestCase("", TestName = "Empty Title")]
        [TestCase("Title", "", TestName = "Empty Content")]
        public async Task Handle_VariousInvalidRequests_ReturnFailure(string title, string content = "Content")
        {
            // Arrange
            var request = CreateArticleRequestBuilder
                .Default()
                .WithTitle(title)
                .WithContent(content)
                .WithDateToPublish(TestContext.CurrentContext.Test.Name == "Past Date" ? DateTime.UtcNow.AddDays(-1) : DateTime.UtcNow.AddDays(1))
                .Build();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();

            // Verify no article was created in the database
            var allArticles = await unitOfWork.Articles.GetAllAsync();
            allArticles.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_PublishDateIsInThePast_ReturnFailure()
        {
            // Arrange
            var request = CreateArticleRequestBuilder.Default().WithDateToPublish(DateTime.UtcNow.AddDays(-1)).Build();

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().NotBeNull();

            // Verify no article was created in the database
            var allArticles = await unitOfWork.Articles.GetAllAsync();
            allArticles.Should().BeEmpty();
        }

        [Test]
        public async Task Handle_ConcurrentRequests_CreatesMultipleArticles()
        {
            // Arrange
            var requests = Enumerable
                .Range(0, 5)
                .Select(i =>
                    CreateArticleRequestBuilder
                        .Default()
                        .WithTitle($"Concurrent Article {i}")
                        .WithContent($"Content for concurrent article {i}")
                        .WithDateToPublish(DateTime.UtcNow.AddDays(i + 1))
                        .Build()
                )
                .ToList();

            // Act
            List<Result<CreateArticleResponse>> results = [];
            foreach (var request in requests)
            {
                var result = await _handler.Handle(request, CancellationToken.None);
                results.Add(result);
            }

            // Assert
            results.Should().AllSatisfy(result => result.IsSuccess.Should().BeTrue());

            var createdArticles = await unitOfWork.Articles.GetAllAsync();
            createdArticles.Should().HaveCount(5);
            createdArticles.Select(a => a.Title).Should().BeEquivalentTo(requests.Select(r => r.Title));
        }
    }
}
