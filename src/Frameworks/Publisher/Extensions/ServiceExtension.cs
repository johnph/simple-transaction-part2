namespace Publisher.Framework.Extensions
{
    using EasyNetQ;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Publisher.Framework.Commands;
    using Publisher.Framework.Configurations;
    using Publisher.Framework.Services;

    public static class ServiceExtension
    {
        public static IServiceCollection AddSingletonCommandPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddSingleton<ICommandPublisher, CommandPublisher>();
            services.TryAddSingleton<IPublishService, PublishService>();
            services.AddServiceConfig(configuration);
            return services;
        }

        public static IServiceCollection AddScopedCommandPublisher(this IServiceCollection services, IConfiguration configuration)
        {
            services.TryAddScoped<ICommandPublisher, CommandPublisher>();
            services.TryAddScoped<IPublishService, PublishService>();
            services.AddServiceConfig(configuration);
            return services;
        }

        private static IServiceCollection AddServiceConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton(RabbitHutch.CreateBus(configuration.GetValue<string>("RabbitMQ:ConnectionString")));
            services.Configure<QueueNames>(options => configuration.GetSection("QueueNames").Bind(options));
            return services;
        }
    }
}
