using Notadesigner.Tommy.App.Properties;

namespace Notadesigner.Tommy.App
{
    public class GuiRunnerContext : ApplicationContext
    {
        private readonly MainForm _form;

        private readonly NotifyIcon _notifyIcon;

        private readonly ContextMenuStrip _contextMenu = new();

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

            var exitMenu = new ToolStripMenuItem("E&xit");
            exitMenu.Click += ExitMenuClickHandler;
            _contextMenu.Items.Add(exitMenu);
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
                _contextMenu.Show(Cursor.Position);
            }
        }

        private void FormClosingHandler(object? sender, FormClosingEventArgs e)
        {
            _form.Hide();
            e.Cancel = true;
        }

        private void ExitMenuClickHandler(object? sender, EventArgs e)
        {
            ExitThread();
        }
    }
}