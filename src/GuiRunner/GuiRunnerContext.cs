using Notadesigner.Pomodour.App.Properties;

namespace Notadesigner.Pomodour.App
{
    public class GuiRunnerContext : ApplicationContext
    {
        private readonly MainForm _form;

        private readonly NotifyIcon _notifyIcon;

        public GuiRunnerContext(MainForm form)
        {
            _form = form;
            _form.FormClosing += FormClosingHandler;

            _notifyIcon = new NotifyIcon
            {
                Icon = GuiRunnerResources.MainIcon,
                Visible = true
            };

            _notifyIcon.MouseClick += NotifyIconMouseClickHandler;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            
            _notifyIcon.Dispose();
        }

        private void NotifyIconMouseClickHandler(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _form.Show();
            }
            else if (e.Button == MouseButtons.Right)
            {
                /// Show context menu
            }
        }

        private void FormClosingHandler(object? sender, FormClosingEventArgs e)
        {
            _form.Hide();
            e.Cancel = true;
        }
    }
}