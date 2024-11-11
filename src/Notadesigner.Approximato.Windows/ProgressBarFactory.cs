using Notadesigner.Approximato.Windows.Controls;

namespace Notadesigner.Approximato.Windows
{
    public static class ProgressBarFactory
    {
        public static TextProgressBar Create(Color color)
        {
            var progressBar = new TextProgressBar()
            {
                DisplayMode = TextProgressBarDisplayMode.Text,
                ForeColor = color,
                Text = "__:__",
                Width = 230,
                Margin = new(0, 0, 0, 12)
            };

            return progressBar;
        }
    }
}