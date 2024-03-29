using ServerlessAPI.Entities;

namespace ServerlessAPI.Repositories;

public class BookRepository : IBookRepository
{
    private readonly ILogger<BookRepository> _logger;
    private readonly IList<Book> _books;

    public BookRepository(ILogger<BookRepository> logger)
    {
        _books = new List<Book>();
        
        _books.Add(new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book 1",
            ISBN = "1234567890",
            Authors = new List<string> { "Author 1", "Author 2" },
            CoverPage = "https://example.com/coverpage1.jpg"
        });
        
        _books.Add(new Book
        {
            Id = Guid.NewGuid(),
            Title = "Book 2",
            ISBN = "0987654321",
            Authors = new List<string> { "Author 3", "Author 4" },
            CoverPage = "https://example.com/coverpage2.jpg"
        });
        
        _logger = logger;
    }

    public async Task<bool> CreateAsync(Book book)
    {
        try
        {
            book.Id = Guid.NewGuid();
            _books.Add(book);
            _logger.LogInformation("Book {} is added", book.Id);
            await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to persist to Table");
            return false;
        }

        return true;
    }

    public async Task<bool> DeleteAsync(Book book)
    {
        bool result;
        try
        {
            var deletedBook = _books.FirstOrDefault(b => b.Id == book.Id);
            _books.Remove(deletedBook);
            // Try to retrieve deleted book. It should return null.
            deletedBook = _books.FirstOrDefault(b => b.Id == book.Id);
            result = deletedBook == null;
            await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to delete book from Table");
            result = false;
        }

        if (result) _logger.LogInformation("Book {Id} is deleted", book);

        return result;
    }

    public async Task<bool> UpdateAsync(Book book)
    {
        if (book == null) return false;

        try
        {
            var existingBook = _books.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                _logger.LogWarning("Book {Id} not found", book);
                return false;
            }
            
            existingBook.Title = book.Title;
            existingBook.ISBN = book.ISBN;
            existingBook.Authors = book.Authors;
            existingBook.CoverPage = book.CoverPage;
            _books.Remove(existingBook);
            _books.Add(existingBook);
            _logger.LogInformation("Book {Id} is updated", book);
            await Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from Table");
            return false;
        }

        return true;
    }

    public async Task<Book?> GetByIdAsync(Guid id)
    {
        try
        {
            var book = _books.FirstOrDefault(b => b.Id == id);
            return await Task.FromResult(book);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to update book from Table");
            return null;
        }
    }

    public async Task<IList<Book>> GetBooksAsync(int limit = 10)
    {
        var result = new List<Book>();

        try
        {
            if (limit <= 0)
            {
                return result;
            }
            
            result = _books.Take(limit).ToList();
            await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "fail to list books from Table");
            return new List<Book>();
        }

        return result;
    }
}