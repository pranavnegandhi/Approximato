using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notadesigner.Approximato.Core;
using Notadesigner.Approximato.Messaging.ServiceRegistration;
using Notadesigner.Approximato.Windows.Properties;
using Serilog;
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
                    .ConfigureServices(ConfigureServicesDelegate)
                    .UseSerilog();

                var host = builder.Build();
                await host.Services.StartConsumers();

                ApplicationConfiguration.Initialize();

                await host.RunAsync();
            }
            catch (ApplicationException exception)
            {
                Log.Fatal(exception, "Approximato was not launched correctly.");
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "Error encountered while running Approximato.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static void ConfigureServicesDelegate(HostBuilderContext context, IServiceCollection collection)
        {
            var appSettings = GuiRunnerSettings.Default;
            StateHostSettings settingsFactory() => new(appSettings.MaximumRounds,
                appSettings.FocusDuration,
                appSettings.ShortBreakDuration,
                appSettings.LongBreakDuration,
                appSettings.LenientMode);

            collection.AddSingleton(settingsFactory)
                .AddSingleton<MainForm>()
                .AddSingleton<SettingsForm>()
                .CreateEvent<UIEvent>()
                .AddEventHandler<UIEvent, StateHost>()
                .CreateEvent<TransitionEvent>()
                .AddEventHandler<TransitionEvent, GuiTransitionEventHandler>("guiTransition")
                .CreateEvent<TimerEvent>()
                .AddEventHandler<TimerEvent, GuiTimerEventHandler>();
        }
    }
}