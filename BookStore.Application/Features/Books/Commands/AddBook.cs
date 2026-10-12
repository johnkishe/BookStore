using BookStore.Application.Common;
using BookStore.Application.Interfaces;
using BookStore.Domain.Entity;
using BookStore.Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BookStore.Application.Features.Books.Commands
{
    public record AddBookCommand(
        string Title,
        string Author, 
        string OpenLibraryKey,
        string? ISBN,
        int? PublishedYear,
        string? Description,
        string? CoverUrl,
        GenreEnum Genre,
        decimal Price,
        int Stock) : IRequest<BookResponse>;

    public class AddBookValidator : AbstractValidator<AddBookCommand>
    {
        public AddBookValidator()
        {
            RuleFor(x => x.OpenLibraryKey).NotEmpty();
            RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Author).NotEmpty().MaximumLength(500);
            RuleFor(x => x.Description).MaximumLength(4000);
            RuleFor(x => x.Genre).IsInEnum();
            RuleFor(x => x.Price).InclusiveBetween(0, 10000);
            RuleFor(x => x.Stock).InclusiveBetween(0, 100000);
        }
    }
    public class AddBookHandler(IAppDbContext db, ILogger<AddBookHandler> logger) : IRequestHandler<AddBookCommand, BookResponse>
    {
        public async Task<BookResponse> Handle(AddBookCommand request, CancellationToken cancellationToken)
        {
            // Check if a book with the same OpenLibraryKey or ISBN already exists in the database
            var isbn = string.IsNullOrWhiteSpace(request.ISBN) ? null : request.ISBN;

            // Check if a book with the same OpenLibraryKey or ISBN already exists in the database
            var exists = await db.Books.AnyAsync(
                b => b.OpenLibraryKey == request.OpenLibraryKey || (isbn != null && b.ISBN == isbn),
                cancellationToken);

            // If a book with the same OpenLibraryKey or ISBN exists, throw a ConflictException
            if (exists)
                throw new ConflictException($"'{request.Title}' is already in the store.");

            // Create a new Book entity and add it to the database
            var book = new Book
            {
                OpenLibraryKey = request.OpenLibraryKey,
                Title = request.Title,
                Author = request.Author,
                ISBN = isbn,
                PublishedYear = request.PublishedYear,
                CoverUrl = request.CoverUrl,
                Description = request.Description,
                Genre = request.Genre,
                Price = request.Price,
                Stock = request.Stock
            };

            // Add the new book to the database and save changes
            db.Books.Add(book);
            await db.SaveChangesAsync(cancellationToken);

            // Log the addition of the new book
            logger.LogInformation("Added book {BookId} {Title} ({OpenLibraryKey})", book.Id, book.Title, book.OpenLibraryKey);

            // Return a BookResponse object created from the newly added book entity
            return BookResponse.FromEntity(book);
        }
    }

}
