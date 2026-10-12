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
    }
}
