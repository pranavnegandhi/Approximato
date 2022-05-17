namespace Notadesigner.Pomodour.App
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.TimeRemainingProgressBar = new CircularProgressBar.CircularProgressBar();
            this.StartPomodoro = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TimeRemainingProgressBar
            // 
            this.TimeRemainingProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn;
            this.TimeRemainingProgressBar.AnimationSpeed = 500;
            this.TimeRemainingProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.TimeRemainingProgressBar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TimeRemainingProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TimeRemainingProgressBar.InnerColor = System.Drawing.SystemColors.ControlDark;
            this.TimeRemainingProgressBar.InnerMargin = 0;
            this.TimeRemainingProgressBar.InnerWidth = 0;
            this.TimeRemainingProgressBar.Location = new System.Drawing.Point(6, 6);
            this.TimeRemainingProgressBar.MarqueeAnimationSpeed = 2000;
            this.TimeRemainingProgressBar.Name = "TimeRemainingProgressBar";
            this.TimeRemainingProgressBar.OuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.TimeRemainingProgressBar.OuterMargin = -20;
            this.TimeRemainingProgressBar.OuterWidth = 20;
            this.TimeRemainingProgressBar.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.TimeRemainingProgressBar.ProgressWidth = 20;
            this.TimeRemainingProgressBar.SecondaryFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TimeRemainingProgressBar.Size = new System.Drawing.Size(120, 120);
            this.TimeRemainingProgressBar.StartAngle = 270;
            this.TimeRemainingProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.TimeRemainingProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.TimeRemainingProgressBar.SubscriptText = "";
            this.TimeRemainingProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.TimeRemainingProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.TimeRemainingProgressBar.SuperscriptText = "";
            this.TimeRemainingProgressBar.TabIndex = 0;
            this.TimeRemainingProgressBar.Text = "12:24 / 25:00";
            this.TimeRemainingProgressBar.TextMargin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.TimeRemainingProgressBar.Value = 45;
            // 
            // StartPomodoro
            // 
            this.StartPomodoro.Location = new System.Drawing.Point(425, 12);
            this.StartPomodoro.Name = "StartPomodoro";
            this.StartPomodoro.Size = new System.Drawing.Size(75, 23);
            this.StartPomodoro.TabIndex = 2;
            this.StartPomodoro.Text = "&Start";
            this.StartPomodoro.UseVisualStyleBackColor = true;
            this.StartPomodoro.Click += new System.EventHandler(this.StartPomodoroClickHandler);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 133);
            this.Controls.Add(this.StartPomodoro);
            this.Controls.Add(this.TimeRemainingProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Pomodour";
            this.ResumeLayout(false);

        }

        #endregion

        private CircularProgressBar.CircularProgressBar TimeRemainingProgressBar;
        private Button StartPomodoro;
    }
}