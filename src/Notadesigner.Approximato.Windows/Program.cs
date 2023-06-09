using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Notadesigner.Approximato.Core;
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
                Log.Information("Starting Pomodour process");

                Log.Verbose("Launching {serviceName}", nameof(PomodoroService));
                var builder = Host.CreateDefaultBuilder(args)
                    .UseWindowsFormsLifetime<GuiRunnerContext>()
                    .ConfigureServices((_, services) =>
                    {
                        var appSettings = GuiRunnerSettings.Default;
                        var settingsFactory = () => new PomodoroServiceSettings(appSettings.MaximumRounds, appSettings.FocusDuration, appSettings.ShortBreakDuration, appSettings.LongBreakDuration, appSettings.LenientMode);
                        services.AddSingleton(settingsFactory)
                            .AddHostedService<PomodoroService>()
                            .AddSingleton(provider => Channel.CreateUnbounded<TransitionEvent>())
                            .AddSingleton(provider => Channel.CreateUnbounded<TimerEvent>())
                            .AddSingleton(provider => Channel.CreateUnbounded<UIEvent>())
                            .AddSingleton<MainForm>()
                            .AddSingleton<SettingsForm>()
                            .AddSingleton<GuiRunnerContext>();
                    })
                    .UseSerilog();
                var host = builder.Build();

                ApplicationConfiguration.Initialize();

                await host.RunAsync();
            }
            catch (ApplicationException exception)
            {
                Log.Fatal(exception, "The service was not launched correctly.");
            }
            catch (Exception exception)
            {
                Log.Fatal(exception, "The service failed to start.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
    }
}