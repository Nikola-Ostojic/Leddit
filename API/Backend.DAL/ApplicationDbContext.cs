using Backend.DAL.Entities;
using Backend.DAL.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Backend.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokens { get; set; }
        public DbSet<ThreadEntity> Threads { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public override int SaveChanges()
        {
            AddAuitInfo();
            return base.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            AddAuitInfo();
            return await base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // TODO Try it out
            // ps: Manually update entity state in base repo
            //this.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            // Customize the ASP.NET Core Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Core Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
            builder.SeedDB();
        }

        /// <summary>
        /// Updating Created and Modified fields on every Entry interaction
        /// </summary>
        private void AddAuitInfo()
        {
            var entries = ChangeTracker.Entries().Where(x =>
                x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    (entry.Entity as BaseEntity).Created = DateTime.UtcNow;
                }

                (entry.Entity as BaseEntity).Modified = DateTime.UtcNow;
            }
        }
    }
}