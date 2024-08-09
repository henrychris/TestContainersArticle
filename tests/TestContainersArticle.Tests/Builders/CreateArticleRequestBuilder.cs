using TestContainersArticle.Main.Features.CreateArticle;

namespace TestContainersArticle.Tests.Builders
{
    public class CreateArticleRequestBuilder
    {
        private string _title = "Default Title";
        private string _content = "Default Content";
        private DateTime _dateToPublish = DateTime.UtcNow.AddDays(1);

        public static CreateArticleRequestBuilder Default()
        {
            return new CreateArticleRequestBuilder();
        }

        public CreateArticleRequestBuilder WithTitle(string title)
        {
            _title = title;
            return this;
        }

        public CreateArticleRequestBuilder WithContent(string content)
        {
            _content = content;
            return this;
        }

        public CreateArticleRequestBuilder WithDateToPublish(DateTime dateToPublish)
        {
            _dateToPublish = dateToPublish;
            return this;
        }

        public CreateArticleRequest Build()
        {
            return new CreateArticleRequest
            {
                Title = _title,
                Content = _content,
                DateToPublish = _dateToPublish
            };
        }
    }
}
