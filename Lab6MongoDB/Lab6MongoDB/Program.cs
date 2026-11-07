using Lab6MongoDB;
using Lab6MongoDB.Models;

LibraryContext libraryContext = new LibraryContext();

while (true)
{
    Console.WriteLine("1. Получить книги");
    Console.WriteLine("2. Добавить книгу");
    Console.WriteLine("3. Обновить книгу");
    Console.WriteLine("4. Удалить книгу");
    Console.WriteLine("5. Получить книгу по ID");
    Console.WriteLine("6. Выход");

    switch (Console.ReadLine())
    {
        case "1":
            Console.Clear();
            await ShowBooksAsync(libraryContext);
            break;

        case "2":
            await AddBookAsync(libraryContext);
            break;

        case "3":
            await UpdateBookAsync(libraryContext);
            break;

        case "4":
            await DeleteBookAsync(libraryContext);
            break;

        case "5":
            await ShowBookByIdAsync(libraryContext);
            break;

        default:
            break;
    }
}

static async Task ShowBooksAsync(LibraryContext libraryContext)
{
    Console.WriteLine("Книги: ");
    var books = await libraryContext.GetBooksAsync();
    foreach (var item in books)
    {
        Console.WriteLine(item.ToString());
    }
    Console.WriteLine();
}

static async Task AddBookAsync(LibraryContext libraryContext)
{
    Console.Write("Введите название: ");
    var name = Console.ReadLine();

    Console.Write("Введите автора: ");
    var author = Console.ReadLine();

    int publicationYear;
    while (true)
    {
        Console.Write("Введите год публикации: ");
        string publicationYearData = Console.ReadLine();
        if (int.TryParse(publicationYearData, out publicationYear))
            break;
    }

    Console.Write("Введите жанр: ");
    var genre = Console.ReadLine();

    double rating;
    while (true)
    {
        Console.Write("Введите рейтинг: ");
        string ratingData = Console.ReadLine();
        if (double.TryParse(ratingData, out rating))
            break;
    }

    Book book = new Book()
    {
        Name = name,
        Author = author,
        PublicationYear = publicationYear,
        Genre = genre,
        Rating = rating,
        IsAvailable = true
    };

    await libraryContext.CreateBookAsync(book);
    Console.WriteLine("Добавили книгу");
    await ShowBooksAsync(libraryContext);
}

static async Task UpdateBookAsync(LibraryContext libraryContext)
{
    Console.Write("Введите ID: ");
    var bookId = Console.ReadLine();

    Console.Write("Введите новое название: ");
    var newName = Console.ReadLine();

    Console.Write("Введите нового автора: ");
    var newAuthor = Console.ReadLine();

    int newPublicationYear;
    while (true)
    {
        Console.Write("Введите год публикации: ");
        string publicationYearData = Console.ReadLine();
        if (int.TryParse(publicationYearData, out newPublicationYear))
            break;
    }

    Console.Write("Введите новый жанр: ");
    var newGenre = Console.ReadLine();

    double newRating;
    while (true)
    {
        Console.Write("Введите новый рейтинг: ");
        string ratingData = Console.ReadLine();
        if (double.TryParse(ratingData, out newRating))
            break;
    }

    Book newBook = new Book()
    {
        Id = bookId,
        Name = newName,
        Author = newAuthor,
        PublicationYear = newPublicationYear,
        Genre = newGenre,
        Rating = newRating,
        IsAvailable = true
    };

    await libraryContext.UpdateBookAsync(newBook);
    var updatedBook = await libraryContext.GetBookByIdAsync(bookId);
    Console.WriteLine(updatedBook.ToString());
    Console.WriteLine();
}

static async Task DeleteBookAsync(LibraryContext libraryContext)
{
    Console.Write("Введите ID: ");
    var deleteId = Console.ReadLine();

    await libraryContext.DeleteBookByIdAsync(deleteId);
    Console.WriteLine("Удалена книга");

    Console.WriteLine();
}

static async Task ShowBookByIdAsync(LibraryContext libraryContext)
{
    Console.Write("Введите ID: ");
    var id = Console.ReadLine();

    var selectedBook = await libraryContext.GetBookByIdAsync(id);
    Console.WriteLine(selectedBook.ToString());

    Console.WriteLine();
}