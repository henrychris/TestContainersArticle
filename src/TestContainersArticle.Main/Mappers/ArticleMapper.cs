using TestContainersArticle.Main.Data.Entities;
using TestContainersArticle.Main.Features.CreateArticle;
using TestContainersArticle.Main.Features.GetArticle;

namespace TestContainersArticle.Main.Mappers
{
    public static class ArticleMapper
    {
        internal static Article CreateArticle(CreateArticleRequest request)
        {
            return new Article
            {
                Title = request.Title,
                Content = request.Content,
                DateToPublish = request.DateToPublish
            };
        }

        internal static CreateArticleResponse CreateArticleResponse(Article article)
        {
            return new CreateArticleResponse { ArticleId = article.Id };
        }

        internal static ArticleResponse GetArticleResponse(Article article)
        {
            return new ArticleResponse
            {
                Id = article.Id,
                Content = article.Content,
                Title = article.Title
            };
        }
    }
}
