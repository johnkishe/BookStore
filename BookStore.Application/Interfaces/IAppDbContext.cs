using BookStore.Application.Features.Books.Querys;
using BookStore.Domain.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Interfaces
{

    public interface IAppDbContext
    {
        DbSet<Book> Books { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        Task<List<GenreCount>> GetGenreCountsAsync(CancellationToken cancellationToken);
    }
}
