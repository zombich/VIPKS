using Lab6MongoDB.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Lab6MongoDB
{
    public class LibraryContext
    {
        public LibraryContext()
        {
            _client = new MongoClient("mongodb://localhost:27017");
            _db = _client.GetDatabase("Library");
            _books = _db.GetCollection<Book>("books");
        }
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<Book> _books;

        public async Task<List<Book>> GetBooksAsync() 
            => await _books.Find(_ => true).ToListAsync();

        public async Task<Book> GetBookByIdAsync(string id) 
            => await _books.Find(b => b.Id == id).FirstOrDefaultAsync();

        public async Task CreateBookAsync(Book book) 
            => await _books.InsertOneAsync(book);

        public async Task UpdateBookAsync(Book book)
        {
            var selectedBook = await GetBookByIdAsync(book.Id);
            if (selectedBook is null)
                return;

            var filter = selectedBook.ToBsonDocument<Book>();

            await _books.ReplaceOneAsync(filter, book);
            return;
        }

        public async Task DeleteBookByIdAsync(string id)
        {
            var selectedBook = await GetBookByIdAsync(id);
            if (selectedBook is null)
                return;

            await _books.DeleteOneAsync(selectedBook.ToBsonDocument<Book>());
        }

    }
}
