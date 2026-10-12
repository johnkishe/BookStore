
CREATE TABLE IF NOT EXISTS "Books" (
    "Id" TEXT NOT NULL CONSTRAINT "PK_Books" PRIMARY KEY,
    "Title" TEXT NOT NULL,
    "Author" TEXT NOT NULL,
    "ISBN" TEXT NULL,
    "PublishedYear" INTEGER NULL,
    "CoverUrl" TEXT NULL,
    "OpenLibraryKey" TEXT NOT NULL,
    "Description" TEXT NULL,
    "Genre" TEXT NOT NULL,
    "Price" REAL NOT NULL,
    "Stock" INTEGER NOT NULL
);

CREATE UNIQUE INDEX IF NOT EXISTS "IX_Books_OpenLibraryKey" ON "Books" ("OpenLibraryKey");

INSERT OR IGNORE INTO "Books"
    ("Id", "Title", "Author", "ISBN", "PublishedYear", "CoverUrl", "OpenLibraryKey", "Description", "Genre", "Price", "Stock")
VALUES
    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000001', 'The Hobbit', 'J.R.R. Tolkien', '9780048231260', 1937,
     'https://covers.openlibrary.org/b/id/14627509-M.jpg', '/works/OL27482W', NULL, 'Fantasy', 14.99, 12),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000002', 'Dune', 'Frank Herbert', '9780340839935', 1965,
     'https://covers.openlibrary.org/b/id/11481354-M.jpg', '/works/OL893414W', NULL, 'SciFi', 18.50, 7),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000003', 'The Hound of the Baskervilles', 'Arthur Conan Doyle', '9781849904094', 1902,
     'https://covers.openlibrary.org/b/id/8063264-M.jpg', '/works/OL262454W', NULL, 'Mystery', 9.99, 4),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000004', 'Steve Jobs', 'Walter Isaacson', '1501127624', 2011,
     'https://covers.openlibrary.org/b/id/12374726-M.jpg', '/works/OL16085155W', NULL, 'Biography', 22.00, 3),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000005', 'Sapiens', 'Yuval Noah Harari', '006342200X', 2011,
     'https://covers.openlibrary.org/b/id/8634250-M.jpg', '/works/OL17075811W', NULL, 'NonFiction', 19.95, 9),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000006', 'Charlotte''s Web', 'E. B. White', '9780061228742', 1952,
     'https://covers.openlibrary.org/b/id/8461797-M.jpg', '/works/OL483391W', NULL, 'Children', 8.49, 15),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000007', 'The Guns of August', 'Barbara Tuchman', '9780786165759', 1962,
     'https://covers.openlibrary.org/b/id/6616967-M.jpg', '/works/OL3378527W', NULL, 'History', 17.25, 2),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000008', 'Pride and Prejudice', 'Jane Austen', '1484110986', 1813,
     'https://covers.openlibrary.org/b/id/14348537-M.jpg', '/works/OL66554W', NULL, 'Fiction', 7.99, 20),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000009', 'The Left Hand of Darkness', 'Ursula K. Le Guin', '9780441478026', 1969,
     'https://covers.openlibrary.org/b/id/10618463-M.jpg', '/works/OL59800W', NULL, 'SciFi', 15.99, 5),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000010', 'Neuromancer', 'William Gibson', '9783608504880', 1984,
     'https://covers.openlibrary.org/b/id/283860-M.jpg', '/works/OL27258W', NULL, 'SciFi', 13.50, 0),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000011', 'Gone Girl', 'Gillian Flynn', '9789510407820', 2011,
     'https://covers.openlibrary.org/b/id/8368314-M.jpg', '/works/OL16239762W', NULL, 'Mystery', 12.99, 6),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000012', 'The Name of the Wind', 'Patrick Rothfuss', '9788580410631', 2007,
     'https://covers.openlibrary.org/b/id/11480483-M.jpg', '/works/OL8479867W', NULL, 'Fantasy', 16.75, 8),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000013', 'Educated', 'Tara Westover', '9780525510673', 2018,
     'https://covers.openlibrary.org/b/id/8314077-M.jpg', '/works/OL18139176W', NULL, 'Biography', 18.00, 4),

    ('0B3D5C1E-1A2B-4C3D-8E4F-000000000014', 'The Very Hungry Caterpillar', 'Eric Carle', '9783423079228', 1969,
     'https://covers.openlibrary.org/b/id/7835968-M.jpg', '/works/OL52987W', NULL, 'Children', 6.99, 25);

-- Test Genre data call.
SELECT Genre, COUNT(*) AS Count FROM Books GROUP BY Genre ORDER BY Genre;
