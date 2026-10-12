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
            Ok(await mediator.Send(new GetBookByIdQuery(), ct));

        [HttpPost]
        public async Task<ActionResult<BookResponse>> Add(AddBookCommand command, CancellationToken ct)
        {
            var book = await mediator.Send(command, ct);
            return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<BookResponse>> Update(Guid id, UpdateBookCommand request, CancellationToken ct)
        {            
            return Ok();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
        {
            await mediator.Send(new DeleteBookCommand(), ct);
            return NoContent();
        }
    }
}
