using Library.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Infrastructure
{
    public class LibraryDBContext : DbContext
    {
        // Tables
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<BookAuthor> BookAuthors { get; set; }
        public DbSet<BookLoan> BookLends { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // many to many relationship between Books and Authors

            // BookAuthor's key = 2 foreign keys
            modelBuilder.Entity<BookAuthor>().HasKey(ba => new {ba.BookId, ba.AuthorId});
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Book).WithMany(b => b.BookAuthors)
                .HasForeignKey(ba => ba.BookId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<BookAuthor>().HasOne(ba => ba.Author).WithMany(a => a.BookAuthors)
                .HasForeignKey(ba => ba.AuthorId).OnDelete(DeleteBehavior.Cascade);

            // BookLend table, 1 to many relationship
            modelBuilder.Entity<BookLoan>().HasOne(bl => bl.Book).WithMany().HasForeignKey(bl => bl.BookId);

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "library.db");
            optionsBuilder.UseSqlite($"Data Source={path}");
        }

    }
}
