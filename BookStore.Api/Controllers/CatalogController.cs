using BookStore.Application.Features.Catalog.Query;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Api.Controllers
{
    [ApiController]
    [Route("api/catalog")]
    public class CatalogController(IMediator mediator) : ControllerBase
    {
        // Search for books in Open Library catalog API based on a search query.
        [HttpGet("search")]
        public async Task<ActionResult<CatalogSearchResult>> Search(string? search) =>
            await mediator.Send(new SearchCatalogQuery(search ?? string.Empty));
    }
}
