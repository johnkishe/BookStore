using BookStore.Application.Features.Books;
using BookStore.Application.Features.Books.Commands;
using BookStore.Application.Features.Books.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/books")]
    public class BooksController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<BookResponse>> GetById(Guid id, CancellationToken ct) =>
            Ok(await mediator.Send(new GetBookByIdQuery(id), ct));

        [HttpPost]
        public async Task<ActionResult<BookResponse>> Add(AddBookCommand command, CancellationToken ct)
        {
            // User mediator to send AddBookCommand and return the created book with a 201 status code
            var book = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookCommand request, CancellationToken ct)
        {
            // Update Book Command with the provided id and request data
            var command = new UpdateBookCommand(id, request.Genre, request.Price, request.Stock, request.Description);
            return Ok(await mediator.Send(command, ct));
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            // Use mediator to send DeleteBookCommand
            await mediator.Send(new DeleteBookCommand(id), ct);
            return NoContent();
        }
    }
}
