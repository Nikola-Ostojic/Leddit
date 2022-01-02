using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

namespace Backend.DAL.Tests.Repositories
{
    public abstract class RepositoryUnitTestsBase : IDisposable
    {
        protected readonly ApplicationDbContext DbContext;

        protected RepositoryUnitTestsBase()
        {
            // Setup
            // Create a fresh service provider, and therefore a fresh 
            // InMemory database instance.
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            // Create a new options instance telling the context to use an
            // InMemory database and the new service provider.
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            DbContext = new ApplicationDbContext(options);
        }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
