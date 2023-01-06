using DataAccess;
using Microsoft.EntityFrameworkCore;

namespace Tests
{
    public class DataContextInitializer
    {
        public static DataContext GetContext()
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: "ContextInMemory")
                .Options;

            var context = new DataContext(options);

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            context.Database.EnsureDeletedAsync();
            context.Database.EnsureCreatedAsync();


            return context;
        }
    }
}
