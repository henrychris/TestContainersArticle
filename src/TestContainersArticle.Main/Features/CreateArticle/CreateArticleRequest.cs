using FluentValidation;
using HenryUtils.Extensions;
using HenryUtils.Results;
using MediatR;
using TestContainersArticle.Main.Data.Repositories.UoW;
using TestContainersArticle.Main.Mappers;

namespace TestContainersArticle.Main.Features.CreateArticle
{
    public class CreateArticleRequest : IRequest<Result<CreateArticleResponse>>
    {
        public DateTime DateToPublish { get; set; }
        public string Content { get; set; } = null!;
        public string Title { get; set; } = null!;
    }

    public class CreateArticleHandler(IUnitOfWork unitOfWork, IValidator<CreateArticleRequest> validator, ILogger<CreateArticleHandler> logger)
        : IRequestHandler<CreateArticleRequest, Result<CreateArticleResponse>>
    {
        public async Task<Result<CreateArticleResponse>> Handle(CreateArticleRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var err = validationResult.ToErrorList();
                logger.LogError("Validation failed for request.\nErrors: {errors}.", err);
                return Result<CreateArticleResponse>.Failure(err);
            }

            var article = ArticleMapper.CreateArticle(request);
            await unitOfWork.Articles.AddAsync(article);
            await unitOfWork.CompleteAsync();

            var response = ArticleMapper.CreateArticleResponse(article);
            return Result<CreateArticleResponse>.Success(response);
        }
    }

    public class CreateArticleValidator : AbstractValidator<CreateArticleRequest>
    {
        public CreateArticleValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Content).NotEmpty();
            RuleFor(x => x.DateToPublish).NotEmpty().GreaterThan(DateTime.UtcNow);
        }
    }
}
