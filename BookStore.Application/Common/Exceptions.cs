using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.Application.Common
{
    public class NotFoundException(string message) : Exception(message);

    public class ConflictException(string message) : Exception(message);

    public class OpenLibraryUnavailableException(Exception innerException)
        : Exception("Open Library is not responding right now. Please try again in a moment.", innerException);
}
