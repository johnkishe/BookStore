using BookStore.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Domain.Entity
{
    public class Book
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public  string Title { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public string? ISBN { get; set; } = null;

        public string? Description { get; set; } = null;
        
        public decimal Price { get; set; } = 0.0m;
         
        public int? PublishedYear { get; set; } = null;

        public string? CoverUrl { get; set; } = null;

        public string OpenLibraryKey { get; set; } = string.Empty;

        public int Stock { get; set; } = 0;

        public GenreEnum Genre { get; set; }
    }
}
