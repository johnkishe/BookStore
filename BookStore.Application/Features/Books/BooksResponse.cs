using BookStore.Domain.Entity;
using BookStore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books
{
    public record BookResponse(
    Guid Id,
    string Title,
    string Author,
    string? Isbn,
    int? PublishedYear,
    string? CoverUrl,
    string OpenLibraryKey,
    string? Description,
    GenreEnum Genre,
    decimal Price,
    int Stock)
    {
        public static BookResponse FromEntity(Book book) => new(
            book.Id, book.Title, book.Author, book.ISBN, book.PublishedYear, book.CoverUrl,
            book.OpenLibraryKey, book.Description, book.Genre, book.Price, book.Stock);
    }

    public record PagedResult<T>(List<T> Items, int Page, int PageSize, int TotalCount);
}
