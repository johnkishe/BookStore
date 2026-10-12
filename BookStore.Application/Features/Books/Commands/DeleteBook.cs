using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Features.Books.Commands
{
    public record DeleteBookCommand() : IRequest<BookResponse>;
}
