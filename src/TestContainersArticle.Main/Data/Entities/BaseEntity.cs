﻿namespace TestContainersArticle.Main.Data.Entities
{
    public abstract class BaseEntity
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
        public DateTime DateUpdated { get; set; } = DateTime.UtcNow;
    }
}
