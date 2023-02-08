
namespace JokeApi.Configs;

public class MongoDBConfig {
    public string ConnectionString { get; set; } = null!;
    public string DatabaseName { get; set; } = null!;
    public string UserCollectionName { get; set; } = null!;
    public string JokeCollectionName { get; set; } = null!;

}
