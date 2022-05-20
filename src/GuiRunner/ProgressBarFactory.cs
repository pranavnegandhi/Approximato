namespace Notadesigner.Tom.App
{
    public static class ProgressBarFactory
    {
        public static CircularProgressBar.CircularProgressBar Create()
        {
            var progressBar = new CircularProgressBar.CircularProgressBar
            {
                AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn,
                AnimationSpeed = 500,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
                ForeColor = SystemColors.ControlText,
                InnerColor = SystemColors.Control,
                InnerMargin = 0,
                InnerWidth = 10,
                Location = new System.Drawing.Point(3, 3),
                MarqueeAnimationSpeed = 2000,
                Name = "Pomodoro1ProgressBar",
                OuterColor = SystemColors.ControlDarkDark,
                OuterMargin = -20,
                OuterWidth = 20,
                ProgressColor = SystemColors.Highlight,
                ProgressWidth = 20,
                SecondaryFont = new Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point),
                Size = new Size(120, 120),
                StartAngle = 270,
                SubscriptColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166))))),
                SubscriptMargin = new Padding(10, -35, 0, 0),
                SubscriptText = "",
                SuperscriptColor = Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166))))),
                SuperscriptMargin = new Padding(10, 35, 0, 0),
                SuperscriptText = "",
                TabIndex = 1,
                Text = "__:__ / __:__",
                TextMargin = new Padding(0, 8, 0, 0),
                Value = 0
            };

            return progressBar;
        }
    }
}