namespace Statement.Framework.Extensions
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using MongoDB.Driver;
    using StackExchange.Redis;
    using Statement.Framework.Documents;
    using Statement.Framework.Repositories;
    using Statement.Framework.Services;
    using System;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddStatementFramework(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IStatementService, StatementService>();
            services.AddScoped(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
            services.AddMongoConfiguration<AccountStatement>(configuration);
            services.AddSingleton(typeof(ICacheRepository<>), typeof(CacheRepository<>));
            services.AddRedisConfiguration(configuration);
            services.Configure<CacheExpiry>(options =>
            {
                options.ExpiryInSeconds = int.Parse(configuration.GetSection("CacheExpiryInSeconds").Value);
            });

            return services;
        }

        private static IServiceCollection AddMongoConfiguration<TDocument>(this IServiceCollection services, IConfiguration config)
        {
            // Mongo Config
            var client = new MongoClient(config.GetSection("MongoConfiguration:ConnectionString").Value);
            var database = client.GetDatabase(config.GetSection("MongoConfiguration:Database").Value);
            var collection = database.GetCollection<TDocument>(typeof(TDocument).Name.ToLower());

            services.AddScoped(_ => collection);

            return services;
        }

        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration config)
        {
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(config.GetSection("RedisCache:ConnectionString").Value));
            IDatabase _database = lazyConnection.Value.GetDatabase();
            services.AddSingleton(_database);

            return services;
        }
    }
}
