using BookStore.Application.Interfaces;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Catalog.Query
{
    public record SearchCatalogQuery(string Query) : IRequest<CatalogSearchResult>;

    // The SearchCatalogValidator class validates the SearchCatalogQuery.
    public class SearchCatalogValidator : AbstractValidator<SearchCatalogQuery>
    {
        public SearchCatalogValidator()
        {
            // The query must be at least 2 characters long and no more than 200 characters long.
            RuleFor(x => x.Query).NotEmpty().MinimumLength(2).MaximumLength(200);
        }
    }

    // The CatalogBook record represents a book in the Open Library catalog.
    public record CatalogBook(
        string OpenLibraryKey,
        string Title,
        string Author,
        string? Isbn,
        int? PublishedYear,
        string? CoverUrl);

    public record CatalogSearchResult(List<CatalogBook> Items, int TotalFound);

    // Search results are only returned to the client, never saved.
    public class SearchCatalogHandler(IOpenLibraryService openLibrary) : IRequestHandler<SearchCatalogQuery, CatalogSearchResult>
    {
        // The handler uses the IOpenLibraryService to search for books in the Open Library catalog.
        public Task<CatalogSearchResult> Handle(SearchCatalogQuery request, CancellationToken cancellationToken) =>
            openLibrary.SearchAsync(request.Query.Trim(), cancellationToken);
    }
}
