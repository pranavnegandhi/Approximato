namespace Notadesigner.Tom.App
{
    public static class ProgressBarFactory
    {
        public static CircularProgressBar.CircularProgressBar Create(Color progressColor, Size size)
        {
            var progressBar = new CircularProgressBar.CircularProgressBar
            {
                AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn,
                AnimationSpeed = 50,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 8F, FontStyle.Regular, GraphicsUnit.Point),
                ForeColor = SystemColors.ControlText,
                InnerColor = SystemColors.Control,
                InnerMargin = 0,
                InnerWidth = 10,
                Location = new Point(3, 3),
                MarqueeAnimationSpeed = 2000,
                Name = "Pomodoro1ProgressBar",
                OuterColor = SystemColors.ControlDarkDark,
                OuterMargin = -10,
                OuterWidth = 10,
                ProgressColor = progressColor,
                ProgressWidth = 10,
                SecondaryFont = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point),
                Size = size,
                StartAngle = 270,
                SubscriptColor = Color.FromArgb(166, 166, 166),
                SubscriptMargin = new Padding(10, -35, 0, 0),
                SubscriptText = "",
                SuperscriptColor = Color.FromArgb(166, 166, 166),
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