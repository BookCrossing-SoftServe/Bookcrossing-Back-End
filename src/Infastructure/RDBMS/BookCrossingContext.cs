using Domain.RDBMS.Entities;
using Infrastructure.RDBMS.Configuration;
using Infrastructure.RDBMS.Seeder;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.RDBMS
{
    public class BookCrossingContext : DbContext
    {
        public BookCrossingContext(DbContextOptions<BookCrossingContext> options) : base(options)
        {
        }


        public DbSet<User> User { get; set; }
        public DbSet<Author> Author { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<BookAuthor> BookAuthor { get; set; }
        public DbSet<BookGenre> BookGenre { get; set; }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Language> Language { get; set; }
        public DbSet<Location> Location { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<UserRoom> UserRoom { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<ResetPassword> ResetPassword { get; set; }
        public DbSet<ScheduleJob> ScheduleJob { get; set; }
        public DbSet<RefreshToken> RefreshToken { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoomConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookAuthorConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
            modelBuilder.ApplyConfiguration(new ResetPasswordConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleJobConfiguration());
            modelBuilder.ApplyConfiguration(new LanguageConfiguration());
            
            DataSeeder.Seed(modelBuilder);
        }
    }
}
