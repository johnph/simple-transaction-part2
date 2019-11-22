using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using Publisher.Framework.Extensions;
using Receiver.Service.Clients;
using Receiver.Service.Documents;
using Receiver.Service.Helpers;
using Receiver.Service.Processors;
using Receiver.Service.Receivers;
using Receiver.Service.Repository;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace Receiver.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddCommandConfiguration(Configuration)
                .AddReceiverConfiguration()
                .AddProcessorConfiguration()
                .AddOptionsConfiguration(Configuration)
                .AddHttpClientConfiguration(Configuration)
                .AddMongoConfiguration<AccountStatement>(Configuration)
                .AddRedisConfiguration(Configuration);

            services.AddSingleton(typeof(ICacheRepository<>), typeof(CacheRepository<>));
            services.AddSingleton(typeof(IDocumentRepository<>), typeof(DocumentRepository<>));
            services.AddSingleton<IRetryHelper, RetryHelper>();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }
    }

    internal static class ServiceExtensions
    {
        public static IServiceCollection AddReceiverConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, TriggerReceiver>();
            services.AddSingleton<Microsoft.Extensions.Hosting.IHostedService, StatementReceiver>();
            return services;
        }

        public static IServiceCollection AddCommandConfiguration(this IServiceCollection services, IConfiguration config)
        {
            // Command Config
            services.AddSingletonCommandPublisher(config);
            return services;
        }

        public static IServiceCollection AddRedisConfiguration(this IServiceCollection services, IConfiguration config)
        {
            var lazyConnection = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(config.GetValue<string>("RedisCache:ConnectionString")));
            IDatabase _database = lazyConnection.Value.GetDatabase();
            services.AddSingleton(_database);

            return services;
        }

        public static IServiceCollection AddMongoConfiguration<TDocument>(this IServiceCollection services, IConfiguration config)
        {
            // Mongo Config
            var client = new MongoClient(config.GetSection("MongoConfiguration:ConnectionString").Value);
            var database = client.GetDatabase(config.GetSection("MongoConfiguration:Database").Value);
            var collection = database.GetCollection<TDocument>(typeof(TDocument).Name.ToLower());
            
            services.AddScoped(_ => collection);

            return services;
        }

        public static IServiceCollection AddProcessorConfiguration(this IServiceCollection services)
        {
            services.AddSingleton<TriggerProcessor>();
            services.AddSingleton<StatementProcessor>();

            services.AddSingleton<Func<string, IMessageProcessor>>(key =>
            {
                var serviceProvider = services.BuildServiceProvider();

                switch (key)
                {
                    case "trigger":
                        return serviceProvider.GetService<TriggerProcessor>();

                    case "statement":
                        return serviceProvider.GetService<StatementProcessor>();

                    default:
                        throw new KeyNotFoundException();
                }
            });

            return services;
        }

        public static IServiceCollection AddOptionsConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<BaseUrlSettings>(options => config.GetSection("BaseUrlSettings").Bind(options));
            services.Configure<CacheExpiry>(options =>
            {
                options.ExpiryInSeconds = int.Parse(config.GetSection("CacheExpiryInSeconds").Value);
            });
            return services;
        }

        public static IServiceCollection AddNamedHttpClientConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddNamedHttpClient(configuration, () => new HttpConfigAttribs()
            {
                Name = NamedHttpClients.IdentityServiceClient,
                BaseUrl = configuration.GetSection("BaseUrlSettings")["IdentityApiBaseUrl"],
                MediaType = Constants.MediaTypeAppJson
            });

            services.AddNamedHttpClient(configuration, () => new HttpConfigAttribs()
            {
                Name = NamedHttpClients.TransactionServiceClient,
                BaseUrl = configuration.GetSection("BaseUrlSettings")["TransactionApiBaseUrl"],
                MediaType = Constants.MediaTypeAppJson
            });

            return services;
        }

        public static IServiceCollection AddNamedHttpClient(this IServiceCollection services, IConfiguration config, Func<HttpConfigAttribs> action)
        {
            var httpConfig = action();
            services.AddHttpClient(httpConfig.Name, client =>
            {
                client.BaseAddress = new Uri(httpConfig.BaseUrl);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(httpConfig.MediaType));
            });

            return services;
        }

        public static IServiceCollection AddHttpClientConfiguration(this IServiceCollection services, IConfiguration config)
        {
            services.AddNamedHttpClientConfiguration(config);
            services.AddSingleton<IIdentityClient, IdentityClient>();
            services.AddSingleton<ITransactionClient, TransactionClient>();
            return services;
        }
    }
}
