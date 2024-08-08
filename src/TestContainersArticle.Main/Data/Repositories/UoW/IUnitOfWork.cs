namespace TestContainersArticle.Main.Data.Repositories.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IArticleRepository Articles { get; }
        Task<int> CompleteAsync();
    }
}
