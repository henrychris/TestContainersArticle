using TestContainersArticle.Main.Data.Entities;
using TestContainersArticle.Main.Data.Repositories.Base;

namespace TestContainersArticle.Main.Data.Repositories
{
    public interface IArticleRepository : IRepository<Article> { }
}
