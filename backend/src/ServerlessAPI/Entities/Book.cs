namespace ServerlessAPI.Entities;

public class Book
{
    public Guid Id { get; set; } = Guid.Empty;
    public string Title { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public List<string>? Authors { get; set; }
    public string? CoverPage { get; set; }
}
