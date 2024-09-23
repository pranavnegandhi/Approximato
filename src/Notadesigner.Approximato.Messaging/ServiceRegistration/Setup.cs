using Microsoft.Extensions.DependencyInjection;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Approximato.Messaging.Impl;
using System.Threading.Channels;

namespace Notadesigner.Approximato.Messaging.ServiceRegistration
{
    public static class Setup
    {
        public static IServiceCollection AddInMemoryEvent<T, THandler>(this IServiceCollection services)
            where THandler : class, IEventHandler<T>
        {
            var options = new UnboundedChannelOptions()
            {
                AllowSynchronousContinuations = false,
                SingleReader = false,
                SingleWriter = false
            };
            var bus = Channel.CreateUnbounded<Event<T>>(options);

            services.AddScoped<IEventHandler<T>, THandler>();

            services.AddSingleton<IProducer<T>>(_ => new InMemoryEventBusProducer<T>(bus.Writer));
            InMemoryEventBusConsumer<T> consumerFactory(IServiceProvider provider) => new(bus.Reader, provider);

            services.AddSingleton<IConsumer>((Func<IServiceProvider, InMemoryEventBusConsumer<T>>)consumerFactory);
            services.AddSingleton<IConsumer<T>>((Func<IServiceProvider, InMemoryEventBusConsumer<T>>)consumerFactory);
            services.AddSingleton<IEventContextAccessor<T>, EventContextAccessor<T>>();

            return services;
        }

        public static IServiceProvider StartConsumers(this IServiceProvider services)
        {
            var consumers = services.GetServices<IConsumer>();
            foreach (var consumer in consumers)
            {
                _ = Task.Factory.StartNew(() => consumer.StartAsync(), TaskCreationOptions.LongRunning);
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