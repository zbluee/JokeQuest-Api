
namespace JokeApi.Modles;

public class QueryParameters {
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;

    public string? Title { get; set; }
}