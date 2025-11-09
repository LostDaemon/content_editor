using MongoDB.Driver;

namespace Editor.Infrastructure.Data
{
    public class ApplicationDbContext
    {
        public ApplicationDbContext(IMongoDatabase database)
        {
            Database = database;
        }

        public IMongoDatabase Database { get; }
    }
}

