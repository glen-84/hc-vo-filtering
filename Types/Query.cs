namespace hc_vo_filtering.Types;

[QueryType]
public static class Query
{
    public static Book GetBook()
        => new Book(BookId.From(5294967295), "C# in depth.", new Author("Jon Skeet"));

    [UseFiltering]
    public static IQueryable<Book> GetBooks()
        => new List<Book> { new(BookId.From(5294967295), "C# in depth.", new Author("Jon Skeet")) }.AsQueryable();
}
