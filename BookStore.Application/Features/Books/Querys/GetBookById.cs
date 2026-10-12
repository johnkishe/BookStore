using BookStore.Application.Common;
using BookStore.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Querys
{
    public record GetBookByIdQuery(Guid Id) : IRequest<BookResponse>;

    public class GetBookByIdHandler(IAppDbContext db) : IRequestHandler<GetBookByIdQuery, BookResponse>
    {
        public async Task<BookResponse> Handle(GetBookByIdQuery request, CancellationToken cancellationToken)
        {
            // Use AsNoTracking for read-only queries to improve performance
            var book = await db.Books.AsNoTracking().FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Book {request.Id} was not found.");

            // Map the Book entity to BookResponse DTO
            return BookResponse.FromEntity(book);
        }
    }
}
