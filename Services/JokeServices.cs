using MongoDB.Driver;
using Microsoft.Extensions.Options;
using JokeApi.Modles;
using JokeApi.Configs;

namespace JokeApi.Services;

public class JokeServices : IJokeServices
{
    private readonly IMongoCollection<Joke> _joke;

    public JokeServices(IOptions<MongoDBConfig> mongoDBConfig)
    {
        var clinet = new MongoClient(mongoDBConfig.Value.ConnectionString);
        var database = clinet.GetDatabase(mongoDBConfig.Value.DatabaseName);
        _joke = database.GetCollection<Joke>(mongoDBConfig.Value.JokeCollectionName);
    }

    public async Task<List<Joke>> GetAllAsync(string? userId, int pageSize, int page)
    {
        if (String.IsNullOrWhiteSpace(userId)) throw new NullReferenceException("user Id");
        var filter = Builders<Joke>.Filter.AnyEq("createdBy", userId);
        return await _joke.Find(filter).Skip(pageSize * (page - 1)).Limit(pageSize).ToListAsync();
    }

    public async Task<List<Joke>> GetAllJokes() => await _joke.Find(_ => true).ToListAsync();

    public async Task<Joke> FindJokeById(string id) => await _joke.Find(joke => joke.Id == id).FirstOrDefaultAsync();
    public async Task<Joke> FindJokeByJQ(string jokeQuestion) => await _joke.Find(joke => joke.JokeQuestion == jokeQuestion).FirstOrDefaultAsync();

    public async Task<Joke> GetOneAsync(string? userId, string jokeId)
    {
        if (String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");
        return await _joke.Find(joke => joke.CreatedBy == userId && joke.Id == jokeId).FirstOrDefaultAsync();
    }

    public async Task CreateOneAsync(Joke joke) => await _joke.InsertOneAsync(joke);

    public async Task<Joke> UpdateJoke(string? userId, string jokeId, Joke joke)
    {
        if(String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");
        var filter = Builders<Joke>.Filter.Where(joke => joke.CreatedBy == userId && joke.Id == jokeId);
        var options = new FindOneAndReplaceOptions<Joke>
        {
            ReturnDocument = ReturnDocument.After
        };
        return await _joke.FindOneAndReplaceAsync(filter, joke, options);
    }

    public async Task<Joke> DeleteOne(string? userId, string jokeId)
    {
        if(String.IsNullOrWhiteSpace(userId)) throw new ArgumentNullException("userId");
        var filter = Builders<Joke>.Filter.Where(joke => joke.CreatedBy == userId && joke.Id == jokeId);
        var joke = await _joke.FindOneAndDeleteAsync(filter);
        return joke;
    }

    public async Task<Joke> DeleteOneById(string jokeId)
    {
        var filter = Builders<Joke>.Filter.Where(joke => joke.Id == jokeId);
        var joke = await _joke.FindOneAndDeleteAsync(filter);
        return joke;
    }
    public async Task DeleteAll() => await _joke.DeleteManyAsync(_ => true);
}