using Notadesigner.Tom.App.Properties;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;

namespace Notadesigner.Tom.App
{
    public partial class SettingsForm : Form
    {
        private readonly bool _isInitialized = false;

        public SettingsForm()
        {
            InitializeComponent();

            if (_isInitialized)
            {
                return;
            }

            WorkSessionInput.GotFocus += SessionInputGotFocusHandler;
            ShortBreakInput.GotFocus += SessionInputGotFocusHandler;
            LongBreakInput.GotFocus += SessionInputGotFocusHandler;

            DisplayAssemblyInfo();
            PrepareSettingsBindings();
            _isInitialized = true;
        }

        private void SessionInputGotFocusHandler(object? s, EventArgs e) => (_ = (s as NumericUpDown) ?? null)?.Select(0, 2);

        private void DisplayAssemblyInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var titleAttribute = assembly.GetCustomAttribute<AssemblyTitleAttribute>();
            var versionAttribute = assembly.GetCustomAttribute<AssemblyVersionAttribute>();
            var copyrightAttribute = assembly.GetCustomAttribute<AssemblyCopyrightAttribute>();
            var companyAttribute = assembly.GetCustomAttribute<AssemblyCompanyAttribute>();

            var infoText = $"{titleAttribute?.Title ?? string.Empty}\t" +
                $"{versionAttribute?.Version ?? string.Empty}{Environment.NewLine}" +
                $"{companyAttribute?.Company ?? string.Empty}{Environment.NewLine}" +
                $"{copyrightAttribute?.Copyright ?? string.Empty}";
            AboutInfoLabel.Text = infoText;
        }

        private void PrepareSettingsBindings()
        {
            try
            {
                /// Attempt to copy settings from the previous version of
                /// the application if they haven't already been copied.
                if (!GuiRunnerSettings.Default.IsUpgraded)
                {
                    GuiRunnerSettings.Default.Upgrade();
                    GuiRunnerSettings.Default.IsUpgraded = true;
                }

                /// Set up the binding for use during the runtime. Wrap it up in a
                /// try-catch block to prevent binding exceptions from breaking the
                /// design-time authoring of the control.
                var binding = new Binding(nameof(CheckBox.Checked), GuiRunnerSettings.Default, nameof(GuiRunnerSettings.AutoAdvance), false, DataSourceUpdateMode.OnPropertyChanged, true);
                EnableAutoAdvance.DataBindings.Add(binding);

                binding = new Binding(nameof(NumericUpDown.Value), GuiRunnerSettings.Default, nameof(GuiRunnerSettings.PomodoroDuration), true, DataSourceUpdateMode.OnPropertyChanged, true);
                binding.Format += TimeSpanToMinutesConverter;
                binding.Parse += MinutesToTimeSpanConverter;
                WorkSessionInput.DataBindings.Add(binding);

                binding = new Binding(nameof(NumericUpDown.Value), GuiRunnerSettings.Default, nameof(GuiRunnerSettings.ShortBreakDuration), true, DataSourceUpdateMode.OnPropertyChanged, true);
                binding.Format += TimeSpanToMinutesConverter;
                binding.Parse += MinutesToTimeSpanConverter;
                ShortBreakInput.DataBindings.Add(binding);

                binding = new Binding(nameof(NumericUpDown.Value), GuiRunnerSettings.Default, nameof(GuiRunnerSettings.LongBreakDuration), true, DataSourceUpdateMode.OnPropertyChanged, true);
                binding.Format += TimeSpanToMinutesConverter;
                binding.Parse += MinutesToTimeSpanConverter;
                LongBreakInput.DataBindings.Add(binding);

                GuiRunnerSettings.Default.PropertyChanged += SettingsPropertyChangedHandler;
            }
            catch (Exception exception)
            when (exception is SettingsPropertyNotFoundException ||
                    exception is TargetInvocationException ||
                    exception.InnerException is SettingsPropertyNotFoundException ||
                    exception.InnerException is UriFormatException)
            {
                /// Swallow the exception if thrown in design mode
            }
        }

        private void TimeSpanToMinutesConverter(object? sender, ConvertEventArgs e)
        {
            if (e.DesiredType != typeof(decimal))
            {
                return;
            }

            var duration = (TimeSpan?)e.Value ?? TimeSpan.FromMinutes(1);
            e.Value = Convert.ToDecimal(duration.TotalMinutes);

            Trace.TraceInformation($"{e.Value}\t{duration.TotalMinutes}");
        }

        private void MinutesToTimeSpanConverter(object? sender, ConvertEventArgs e)
        {
            if (e.DesiredType != typeof(TimeSpan))
            {
                return;
            }

            var minutes = (decimal?)e.Value ?? 1M;
            e.Value = TimeSpan.FromMinutes(Convert.ToDouble(minutes));

            Trace.TraceInformation($"{e.Value}\t{e.Value}");
        }

        private void SaveSettingsClickHandler(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Invoked by the <see cref="Settings"/> object when any of its properties
        /// are changed. The handler invokes the <see cref="ApplicationSettingsBase.Save"/>
        /// method on the default settings instance so that the values are committed
        /// to disk.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">A System.ComponentModel.PropertyChangedEventArgs that contains the event data.</param>
        private void SettingsPropertyChangedHandler(object? sender, PropertyChangedEventArgs e)
        {
            GuiRunnerSettings.Default.Save();
        }
    }
}