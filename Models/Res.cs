namespace JokeApi.Modles;

public class Res {

    public bool Success { get; set; }      
    public int Count { get; set; }
    public List<Joke> Joke { get; set; } = null!;
    

}