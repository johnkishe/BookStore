using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Querys
{
    public record GetBookByIdQuery() : IRequest<BookResponse>;
}
