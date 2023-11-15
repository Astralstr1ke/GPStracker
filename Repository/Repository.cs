using GPStracker.Model;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace GPStracker.Repository
{
    public interface IRepository
    {
        public Task Add(Package package);
        public Task Delete(Guid id);

        public Task<Package?> Get(Guid id);
        public Task<List<Package>> GetAll();

    }


    public class PackageRepository : IRepository
    {
        private readonly IMongoCollection<Package> _packages;


        public PackageRepository(IOptions<MongoDBRestSettings> mongoDBRest)
        {
            var mongoClient = new MongoClient(mongoDBRest.Value.ConnectionString);

            var mongoDatabase = mongoClient.GetDatabase(mongoDBRest.Value.DatabaseName);

            _packages = mongoDatabase.GetCollection<Package>(mongoDBRest.Value.PackageCollectionName);

            
        }

        public async Task Add(Package package)
        {
            await _packages.InsertOneAsync(package);
        }

        public async Task<Package?> Get(Guid id)
        {
            return await _packages.Find(s => s.Id == id).FirstOrDefaultAsync();

        }

        public async Task<List<Package>> GetAll()
        {
            return await _packages.Find(s => true).ToListAsync();
        }

        public async Task Delete(Guid id)
        {
            await _packages.DeleteOneAsync(s => s.Id == id);
        }

    }
}
