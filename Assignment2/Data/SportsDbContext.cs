using Assignment2.Models;
using Microsoft.EntityFrameworkCore;

namespace Assignment2.Data
{
    public class SportsDbContext : DbContext
    {
        public DbSet<Fan> Fans { get; set; }
        public DbSet<Subscription> Subscriptions { get; set; }
        public DbSet<SportClub> SportClub { get; set; }
        public DbSet<News> News { get; set; }


        public SportsDbContext(DbContextOptions<SportsDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Fan>().ToTable("Fan");
            modelBuilder.Entity<Subscription>().ToTable("Subscription");
            modelBuilder.Entity<SportClub>().ToTable("SportClub");
            modelBuilder.Entity<News>().ToTable("news");
            modelBuilder.Entity<Subscription>()
                .HasKey(s => new { s.FanID, s.SportClubID });
        }


    }
}
