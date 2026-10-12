using BookStore.Application.Interfaces;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Querys
{
    public record GetGenreCountsQuery : IRequest<List<GenreCount>>;

    public class GenreCount
    {
        public string Genre { get; set; } = string.Empty;
        public int Count { get; set; }
    }

    public class GetGenreCountsHandler(IAppDbContext db) : IRequestHandler<GetGenreCountsQuery, List<GenreCount>>
    {
        public Task<List<GenreCount>> Handle(GetGenreCountsQuery request, CancellationToken cancellationToken) =>
            db.GetGenreCountsAsync(cancellationToken);
    }
}
