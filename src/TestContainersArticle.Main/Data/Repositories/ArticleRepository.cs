using TestContainersArticle.Main.Data.Entities;
using TestContainersArticle.Main.Data.Repositories.Base;

namespace TestContainersArticle.Main.Data.Repositories
{
    public class ArticleRepository(DataContext context) : BaseRepository<Article>(context), IArticleRepository { }
}
