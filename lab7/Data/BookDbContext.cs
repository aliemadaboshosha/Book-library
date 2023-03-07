using lab7.Models;
using Microsoft.EntityFrameworkCore;

namespace lab7.Data
{
    public class BookDbContext:DbContext
    {public virtual DbSet<Book> Books { get; set; }
        public virtual DbSet<Author> Authors { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }
        public virtual DbSet<Book_Authors> Book_Authors { get; set; }
        public virtual DbSet<PriceOffer> PriceOffers { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-NP9ERV5\\SQLEXPRESS;Database=_Book_System;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<Book_Authors>(a =>
            {
                a.HasKey(a => new { a.Book_id, a.Author_id });
            });
            base.OnModelCreating(modelBuilder);
        }
    }
}
