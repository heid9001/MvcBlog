using System.Data.Entity;


namespace BlogMVC.Models
{
    public class ModelsContext: DbContext
    {
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Articles)
                .WithOptional(u => u.User)
                .WillCascadeOnDelete(true);
        }
    }
}