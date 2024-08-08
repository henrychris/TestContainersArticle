namespace TestContainersArticle.Main.Data.Repositories.UoW
{
    public class UnitOfWork(DataContext context) : IUnitOfWork
    {
        public IArticleRepository Articles => new ArticleRepository(context);

        public async Task<int> CompleteAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                context.Dispose();
            }
        }
    }
}
