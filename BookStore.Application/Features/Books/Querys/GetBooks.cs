using BookStore.Application.Interfaces;
using BookStore.Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Application.Features.Books.Querys
{
    public record GetBooksQuery(GenreEnum? Genre, int Page, int PageSize, string? SortBy, bool Descending)
    : IRequest<PagedResult<BookResponse>>;

    public class GetBooksValidator : AbstractValidator<GetBooksQuery>
    {
        public GetBooksValidator()
        {
            RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
        }
    }

    public class GetBooksHandler(IAppDbContext db) : IRequestHandler<GetBooksQuery, PagedResult<BookResponse>>
    {
        public async Task<PagedResult<BookResponse>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
        {
            // Start with the base query
            var query = db.Books.AsNoTracking();

            // Apply filtering based on the request parameters
            if (request.Genre.HasValue)
                query = query.Where(b => b.Genre == request.Genre.Value);

            // Get the total count before applying pagination
            var totalCount = await query.CountAsync(cancellationToken);

            // Apply sorting based on the request parameters
            query = request.SortBy?.ToLowerInvariant() switch
            {
                "author" => request.Descending ? query.OrderByDescending(b => b.Author) : query.OrderBy(b => b.Author),
                "publishedyear" => request.Descending ? query.OrderByDescending(b => b.PublishedYear) : query.OrderBy(b => b.PublishedYear),
                "genre" => request.Descending ? query.OrderByDescending(b => b.Genre) : query.OrderBy(b => b.Genre),
                "price" => request.Descending ? query.OrderByDescending(b => b.Price) : query.OrderBy(b => b.Price),
                "stock" => request.Descending ? query.OrderByDescending(b => b.Stock) : query.OrderBy(b => b.Stock),
                _ => request.Descending ? query.OrderByDescending(b => b.Title) : query.OrderBy(b => b.Title),
            };

            // Apply pagination
            var books = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync(cancellationToken);

            // Return the paged result
            return new PagedResult<BookResponse>(books.Select(BookResponse.FromEntity).ToList(), request.Page, request.PageSize, totalCount);
        }
    }
}
