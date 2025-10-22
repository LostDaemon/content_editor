using MongoDB.Driver;

namespace UniversalContentEditor.Data
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
