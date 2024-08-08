namespace TestContainersArticle.Main.Data.Entities
{
    public class Article : BaseEntity
    {
        public required string Title { get; set; }
        public required string Content { get; set; }

        /// <summary>
        /// We allow this as input just so we can test the database's response
        /// </summary>
        public required DateTime DateToPublish { get; set; }
    }
}
