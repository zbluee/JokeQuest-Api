using MongoDB.Driver;
using Microsoft.Extensions.Options;
using JokeApi.Modles;
using JokeApi.Configs;

namespace JokeApi.Services;

public class JokeServices
{
    private readonly IMongoCollection<Joke> _joke;

    public JokeServices(IOptions<MongoDBConfig> mongoDBConfig)
    {
        var clinet = new MongoClient(mongoDBConfig.Value.ConnectionString);
        var database = clinet.GetDatabase(mongoDBConfig.Value.DatabaseName);
        _joke = database.GetCollection<Joke>(mongoDBConfig.Value.JokeCollectionName);
    }

    public async Task<List<Joke>> GetAllAsync(string userId, int pageSize, int page)
    {
        var filter = Builders<Joke>.Filter.AnyEq("createdBy", userId);
        return await _joke.Find(filter).Skip(pageSize * (page - 1)).Limit(pageSize).ToListAsync();
    }

    public async Task<Joke> GetOneAsync(string userId, string jokeId) => await _joke.Find(joke => joke.CreatedBy == userId && joke.Id == jokeId).FirstOrDefaultAsync();
    public async Task CreateOneAsync(Joke joke) => await _joke.InsertOneAsync(joke);

    public async Task<Joke> UpdateJoke(string userId, string jokeId, Joke joke)
    {
        var filter = Builders<Joke>.Filter.Where(joke => joke.CreatedBy == userId && joke.Id == jokeId);
        var options = new FindOneAndReplaceOptions<Joke>
        {
            ReturnDocument = ReturnDocument.After
        };
        return await _joke.FindOneAndReplaceAsync(filter, joke, options);
    }

    public async Task DeleteOne(string userId, string jokeId)
    {
        var filter = Builders<Joke>.Filter.Where(joke => joke.CreatedBy == userId && joke.Id == jokeId);
        await _joke.DeleteOneAsync(filter);
    }
    public async Task DeleteAll() => await _joke.DeleteManyAsync(_ => true);
}