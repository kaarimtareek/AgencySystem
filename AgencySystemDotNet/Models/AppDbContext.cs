using System;
using System.Data.Entity;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace PressAgencyApp.Models
{
    public class AppDbContext : DbContext
    {
        private static string _connectinString;

        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(
           CancellationToken cancellationToken = default(CancellationToken)
        )
        {
            OnBeforeSaving();
            return (await base.SaveChangesAsync(
                          cancellationToken));
        }

        private void OnBeforeSaving()
        {
            var entries = ChangeTracker.Entries();
            var utcNow = DateTime.UtcNow;

            foreach (var entry in entries)
            {
                // for entities that inherit from BaseEntity,
                // set UpdatedOn / CreatedOn appropriately
                if (entry.Entity is BaseEntity trackable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            // set the updated date to "now"
                            trackable.ModifiedAt = utcNow;

                            // mark property as "don't touch"
                            // we don't want to update on a Modify operation
                            //entry.Property("CreatedOn").IsModified = false;
                            break;

                        case EntityState.Added:
                            // set both updated and created date to "now"
                            trackable.CreatedAt = utcNow;
                            trackable.ModifiedAt = utcNow;
                            break;
                    }
                }
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Post>().HasMany(x=>x.Questions).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<PostQuestion>().HasRequired(x => x.Post).WithMany(x => x.Questions).WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>().HasMany(x=>x.Interactions).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<PostInteraction>().HasRequired(x => x.Post).WithMany(x => x.Interactions).WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>().HasMany(x=>x.Views).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<PostView>().HasRequired(x => x.Post).WithMany(x => x.Views).WillCascadeOnDelete(false);
            modelBuilder.Entity<Post>().HasMany(x=>x.Saved).WithRequired().WillCascadeOnDelete(false);
            modelBuilder.Entity<SavedPost>().HasRequired(x => x.Post).WithMany(x => x.Saved).WillCascadeOnDelete(false);
        }

        public AppDbContext()
        {
        }

        //static AppDbContext()
        //{
        //    var configuration = new ConfigurationBuilder()
        //  .SetBasePath(Directory.GetCurrentDirectory())
        //  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        //  .Build();
        //    _connectinString = configuration.GetConnectionString("AppDB");
        //}

        

        public DbSet<LookupPostCategory> LookupPostCategories { get; set; }
        public DbSet<LookupPostStatus> LookupPostStatuses { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostInteraction> PostInteractions { get; set; }
        public DbSet<PostQuestion> PostQuestions { get; set; }
        public DbSet<PostView> PostViews { get; set; }
        public DbSet<SavedPost> SavedPosts { get; set; }
        public DbSet<User> Users { get; set; }

        public System.Data.Entity.DbSet<PressAgencyApp.ViewModels.Post.PostViewModelR> PostViewModelRs { get; set; }

        public System.Data.Entity.DbSet<PressAgencyApp.ViewModels.Editor.EditorViewModelR> EditorViewModelRs { get; set; }

        public System.Data.Entity.DbSet<PressAgencyApp.ViewModels.Customer.CustomerViewModelR> CustomerViewModelRs { get; set; }

        public System.Data.Entity.DbSet<AgencySystemDotNet.ViewModels.Admin.AdminViewModelR> AdminViewModelRs { get; set; }
    }
}