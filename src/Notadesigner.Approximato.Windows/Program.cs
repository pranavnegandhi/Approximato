using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.Contracts;
using Notadesigner.Approximato.Messaging.ServiceRegistration;
using Notadesigner.Approximato.Windows.Properties;
using Serilog;
using System.Threading.Channels;
using WindowsFormsLifetime;

namespace Notadesigner.Approximato.Windows
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        public static async Task Main(string[] args)
        {
            /// Initialize the application logging framework
            var configurationBuilder = new ConfigurationBuilder()
                .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
            var configuration = configurationBuilder.Build();
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .CreateLogger();

            try
            {
                /// Attempt to initialize the service
                Log.Information("Starting Approximato");
                var builder = Host.CreateDefaultBuilder(args)
                    .UseWindowsFormsLifetime<GuiRunnerContext>()
                    .ConfigureServices((_, services) =>
                    {
                        var appSettings = GuiRunnerSettings.Default;
                        StateHostSettings settingsFactory() => new(appSettings.MaximumRounds,
                            appSettings.FocusDuration,
                            appSettings.ShortBreakDuration,
                            appSettings.LongBreakDuration,
                            appSettings.LenientMode);

                        services.AddSingleton(settingsFactory)
                            .AddInMemoryEvent<UIEvent, StateHost>()
                            .AddSingleton(provider => Channel.CreateUnbounded<TransitionEvent>())
                            .AddSingleton(provider => Channel.CreateUnbounded<TimerEvent>())
                            .AddSingleton<MainForm>()
                            .AddSingleton<SettingsForm>()
                            .AddSingleton<GuiRunnerContext>();
                    })
                    .UseSerilog();

                var host = builder.Build();
                host.Services.StartConsumers();

                ApplicationConfiguration.Initialize();

                await host.RunAsync();
            }
            catch (ApplicationException exception)
            {
                Log.Fatal(exception, "Approximato was not launched correctly.");
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Error encountered while running Approximator.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}