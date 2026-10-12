using BookStore.Application.Common;
using BookStore.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Commands
{
    public record DeleteBookCommand(Guid Id) : IRequest;

    public class DeleteBookHandler(IAppDbContext db, ILogger<DeleteBookHandler> logger) : IRequestHandler<DeleteBookCommand>
    {
        public async Task Handle(DeleteBookCommand request, CancellationToken cancellationToken)
        {
            // Use AsNoTracking for read-only queries to improve performance
            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Book {request.Id} was not found.");

            // Remove the book from the database
            db.Books.Remove(book);
            await db.SaveChangesAsync(cancellationToken);

            // Log the deletion of the book
            logger.LogInformation("Deleted book {BookId} {Title}", book.Id, book.Title);
        }
    }
}
