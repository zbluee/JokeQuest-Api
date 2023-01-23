namespace JokeApi.Modles;

public class ErrorResponse {
    public bool Success { get; set; }      
    public string? Msg { get; set; }
    public int StatusCode { get; set; }

}