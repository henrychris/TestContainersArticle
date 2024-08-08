using System.ComponentModel.DataAnnotations;

namespace TestContainersArticle.Main.Configuration.Settings
{
    public class DatabaseSettings
    {
        [Required(AllowEmptyStrings = false)]
        public string ConnectionString { get; set; } = null!;
    }
}
