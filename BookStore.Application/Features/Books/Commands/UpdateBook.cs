using BookStore.Application.Common;
using BookStore.Application.Interfaces;
using BookStore.Domain.Enum;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Commands
{
    public record UpdateBookCommand(Guid Id, GenreEnum Genre, decimal Price, int Stock, string? Description) : IRequest<BookResponse>;

    public class UpdateBookValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookValidator()
        {
            RuleFor(x => x.Genre).IsInEnum();
            RuleFor(x => x.Price).InclusiveBetween(0, 10000);
            RuleFor(x => x.Stock).InclusiveBetween(0, 100000);
            RuleFor(x => x.Description).MaximumLength(4000);
        }
    }

    public class UpdateBookHandler(IAppDbContext db) : IRequestHandler<UpdateBookCommand, BookResponse>
    {
        public async Task<BookResponse> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
        {
            // Use AsNoTracking for read-only queries to improve performance
            var book = await db.Books.FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken)
                ?? throw new NotFoundException($"Book {request.Id} was not found.");

            // Update the book properties
            book.Genre = request.Genre;
            book.Price = request.Price;
            book.Stock = request.Stock;
            book.Description = request.Description;

            // Save changes to the database
            await db.SaveChangesAsync(cancellationToken);

            // Return the updated book as a response
            return BookResponse.FromEntity(book);
        }
    }
}
