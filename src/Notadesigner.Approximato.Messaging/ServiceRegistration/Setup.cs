using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Approximato.Messaging.Impl;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.ServiceRegistration
{
    public static class Setup
    {
        public static IServiceCollection CreateEvent<T>(this IServiceCollection services)
        {
            var options = new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = false,
                SingleWriter = false
            };
            var bus = Channel.CreateUnbounded<Event<T>>(options);

            services.AddSingleton<IProducer<T>>(_ => new TransientBusEventProducer<T>(bus.Writer));

            IConsumer<T>? consumer = default;
            IConsumer<T> consumerFactory(IServiceProvider provider)
            {
                if (consumer is not null)
                {
                    return consumer;
                }

                var handlers = provider.GetServices<IEventHandler<T>>();
                var contextAccessor = provider.GetRequiredService<IEventContextAccessor<T>>();

                consumer = new TransientBusEventConsumer<T>(bus.Reader, handlers, contextAccessor);

                return consumer;
            }

            services.AddScoped<IConsumer>(consumerFactory);
            services.AddSingleton(consumerFactory);
            services.AddSingleton<IEventContextAccessor<T>, EventContextAccessor<T>>();

            return services;
        }

        public static IServiceCollection AddEventHandler<T, THandler>(this IServiceCollection services, string key)
            where THandler : class, IEventHandler<T> =>
            services.AddKeyedAndDefaultScoped<IEventHandler<T>, THandler>(key);

        public static IServiceCollection AddEventHandler<T, THandler>(this IServiceCollection services)
            where THandler : class, IEventHandler<T> =>
            services.AddScoped<IEventHandler<T>, THandler>();

        public static async Task<IServiceProvider> StartConsumers(this IServiceProvider services)
        {
            var consumers = services.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.StartAsync();
            }

            return services;
        }

        public static async Task<IServiceProvider> StopConsumersAsync(this IServiceProvider services)
        {
            var consumers = services.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                await consumer.StopAsync().ConfigureAwait(false);
            }

            return services;
        }
    }
}