using BookStore.Application.Common;
using BookStore.Application.Features.Catalog.Query;
using BookStore.Application.Interfaces;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace BookStore.Infrastructure
{
    public class OpenLibraryService(HttpClient http, ILogger<OpenLibraryService> logger) : IOpenLibraryService
    {
        // Searches the Open Library API for books matching the given query.
        public async Task<CatalogSearchResult> SearchAsync(string query, CancellationToken cancellationToken)
        {
            // Open Library search API: https://openlibrary.org/dev/docs/api/search
            var url = QueryHelpers.AddQueryString("search.json", new Dictionary<string, string?>
            {
                ["q"] = query,
                ["fields"] = "key,title,author_name,first_publish_year,isbn,cover_i",
                ["limit"] = "10"
            });

            // Log the search request
            SearchResponse? response;
            try
            {
                response = await http.GetFromJsonAsync<SearchResponse>(url, cancellationToken);
            }
            catch (Exception ex) when (!cancellationToken.IsCancellationRequested)
            {
                // Covers network errors, timeouts and bad responses.
                logger.LogWarning(ex, "Open Library search for {Query} failed", query);
                throw new OpenLibraryUnavailableException(ex);
            }

            // Map the response to CatalogBook objects
            var items = (response?.Docs ?? [])
                .Where(d => d.Key != null && d.Title != null)
                .Select(d => new CatalogBook(
                    d.Key!,
                    d.Title!,
                    d.AuthorName is { Count: > 0 } ? string.Join(", ", d.AuthorName) : "Unknown author",
                    d.Isbn?.FirstOrDefault(),
                    d.FirstPublishYear,
                    d.CoverId.HasValue ? $"https://covers.openlibrary.org/b/id/{d.CoverId}-M.jpg" : null))
                .ToList();

            // Log the search result
            logger.LogInformation("Open Library search for {Query} returned {Count} of {TotalFound} results",
                query, items.Count, response?.NumFound ?? 0);

            // Return the search result
            return new CatalogSearchResult(items, response?.NumFound ?? 0);
        }

        // Private classes to deserialize the Open Library search response
        private class SearchResponse 
        {
            [JsonPropertyName("numFound")] public int NumFound { get; set; }
            [JsonPropertyName("docs")] public List<SearchDoc> Docs { get; set; } = [];
        }

        // Private class to represent a single document in the Open Library search response
        private class SearchDoc
        {
            [JsonPropertyName("key")] public string? Key { get; set; }
            [JsonPropertyName("title")] public string? Title { get; set; }
            [JsonPropertyName("author_name")] public List<string>? AuthorName { get; set; }
            [JsonPropertyName("first_publish_year")] public int? FirstPublishYear { get; set; }
            [JsonPropertyName("isbn")] public List<string>? Isbn { get; set; }
            [JsonPropertyName("cover_i")] public int? CoverId { get; set; }
        }
    }
}
