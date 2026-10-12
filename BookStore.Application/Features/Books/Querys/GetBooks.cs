using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Querys
{
    public record GetBooksQuery() : IRequest<List<BookResponse>>;
}
