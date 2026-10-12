using BookStore.Application.Common;
using BookStore.Application.Features.Books;
using BookStore.Application.Features.Books.Commands;
using BookStore.Domain;
using BookStore.Domain.Entity;
using BookStore.Domain.Enum;
using BookStore.Infrastructure;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace BookStore.Test
{
    public class BookStoreUnitTests : IDisposable
    {

        private readonly SqliteConnection _connection = new("Data Source=:memory:");
        private readonly AppDbContext _db;

        public BookStoreUnitTests()
        {
            _connection.Open();
            _db = new AppDbContext(new DbContextOptionsBuilder<AppDbContext>().UseSqlite(_connection).Options);
            _db.Database.EnsureCreated();
        }

        [Fact]
        public async Task AddBook_saves_the_book()
        {
            var handler = new AddBookHandler(_db, NullLogger<AddBookHandler>.Instance);
            var result = await handler.Handle(NewBook("/works/OL1W", "Dune"), CancellationToken.None);

            var saved = await _db.Books.SingleAsync();
            Assert.Equal(result.Id, saved.Id);
            Assert.Equal("Dune", saved.Title);
            Assert.Equal(GenreEnum.SciFi, saved.Genre);
        }

        [Fact]
        public void AddBookValidator_rejects_missing_title_and_negative_price()
        {
            var command = NewBook("/works/OL1W", "") with { Price = -1 };
            var result = new AddBookValidator().Validate(command);

            Assert.Contains(result.Errors, e => e.PropertyName == "Price");
            Assert.Contains(result.Errors, e => e.PropertyName == "Title"); 
        }

        private static AddBookCommand NewBook(string key, string title) =>
            new(key, title, "Frank Herbert", null, 1965, null, null, GenreEnum.SciFi, 18.50m, 3);
        

        
        private async Task AddAsync(string key, string title, GenreEnum genre, decimal price)
        {
            _db.Books.Add(new Book { OpenLibraryKey = key, Title = title, Author = "Someone", Genre = genre, Price = price });
            await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
            _connection.Dispose();
        }
    }
}
