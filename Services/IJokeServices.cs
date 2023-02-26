using JokeApi.Modles;

namespace JokeApi.Services;

public interface IJokeServices {
    public Task<List<Joke>> GetAllAsync(string? userId, int pageSize, int page);

    public Task<List<Joke>> GetAllJokes();

    public Task<Joke> FindJokeById(string id);

    public Task<Joke> FindJokeByJQ(string jokeQuestion);

    public Task<Joke> GetOneAsync(string? userId, string jokeId);
    public Task CreateOneAsync(Joke joke);

    public Task<Joke> UpdateJoke(string? userId, string jokeId, Joke joke);
    public Task<Joke> DeleteOne(string? userId, string jokeId);

    public Task<Joke> DeleteOneById(string jokeId);
    public Task DeleteAll();
    
}