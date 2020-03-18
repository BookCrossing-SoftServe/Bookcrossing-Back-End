using Domain.Entities;
using Infrastructure.Configuration;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class BookCrossingContext : DbContext
    {
        public BookCrossingContext(DbContextOptions<BookCrossingContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public  DbSet<User> User { get; set; }
        public  DbSet<Author> Author { get; set; }
        public  DbSet<Book> Book { get; set; }
        public  DbSet<BookAuthor> BookAuthor { get; set; }
        public  DbSet<BookGenre> BookGenre { get; set; }
        public  DbSet<Genre> Genre { get; set; }
        public  DbSet<Location> Location { get; set; }
        public  DbSet<Request> Request { get; set; }
        public  DbSet<UserLocation> UserLocation { get; set; }
        public DbSet<Role> Role { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new UserLocationConfiguration());
            modelBuilder.ApplyConfiguration(new LocationConfiguration());
            modelBuilder.ApplyConfiguration(new RequestConfiguration());
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new GenreConfiguration());
            modelBuilder.ApplyConfiguration(new BookGenreConfiguration());
            modelBuilder.ApplyConfiguration(new AuthorConfiguration());
            modelBuilder.ApplyConfiguration(new BookAuthorConfiguration());
            modelBuilder.ApplyConfiguration(new RoleConfiguration());
        }
    }
}
