using BookStore.Application.Interfaces;
using BookStore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Infrastructure
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options), IAppDbContext
    {
        public DbSet<Book> Books => Set<Book>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var book = modelBuilder.Entity<Book>();

            book.Property(b => b.Title).HasMaxLength(500).IsRequired();
            book.Property(b => b.Author).HasMaxLength(500).IsRequired();
            book.Property(b => b.OpenLibraryKey).HasMaxLength(50).IsRequired();
            book.HasIndex(b => b.OpenLibraryKey).IsUnique();

            // Store the genre as text so the table and the raw SQL are readable.
            book.Property(b => b.Genre).HasConversion<string>();

            // SQLite can't sort decimals, so store the price as a REAL.
            book.Property(b => b.Price).HasConversion<double>();
        }
    }
}
