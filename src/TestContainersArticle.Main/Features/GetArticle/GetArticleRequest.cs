using FluentValidation;
using HenryUtils.Extensions;
using HenryUtils.Results;
using MediatR;
using TestContainersArticle.Main.Data.Repositories.UoW;
using TestContainersArticle.Main.Mappers;

namespace TestContainersArticle.Main.Features.GetArticle
{
    public class GetArticleRequest : IRequest<Result<ArticleResponse>>
    {
        public required string ArticleId { get; set; }
    }

    public class Handler(IUnitOfWork unitOfWork, IValidator<GetArticleRequest> validator, ILogger<Handler> logger)
        : IRequestHandler<GetArticleRequest, Result<ArticleResponse>>
    {
        public async Task<Result<ArticleResponse>> Handle(GetArticleRequest request, CancellationToken cancellationToken)
        {
            var validationResult = await validator.ValidateAsync(request, cancellationToken);
            if (!validationResult.IsValid)
            {
                var err = validationResult.ToErrorList();
                logger.LogError("Validation failed for request.\nErrors: {errors}.", err);
                return Result<ArticleResponse>.Failure(err);
            }

            var article = await unitOfWork.Articles.GetByIdAsync(request.ArticleId);
            if (article is null)
            {
                logger.LogError("Article not found. Article id: {articleId}.", request.ArticleId);
                return Result<ArticleResponse>.Failure(Error.NotFound("Article.NotFound", "Article not found."));
            }

            var response = ArticleMapper.GetArticleResponse(article);
            return Result<ArticleResponse>.Success(response);
        }

        public class Validator : AbstractValidator<GetArticleRequest>
        {
            public Validator()
            {
                RuleFor(x => x.ArticleId).NotEmpty();
            }
        }
    }
}
