using BookStore.Application.Features.Catalog.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Interfaces
{
    public interface IOpenLibraryService
    {        
        Task<CatalogSearchResult> SearchAsync(string query, CancellationToken cancellationToken);
    }
}
