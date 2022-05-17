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
            this.StartPomodoro = new System.Windows.Forms.Button();
            this.ProgressBarsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.Pomodoro1ProgressBar = new CircularProgressBar.CircularProgressBar();
            this.Pomodoro2ProgressBar = new CircularProgressBar.CircularProgressBar();
            this.circularProgressBar1 = new CircularProgressBar.CircularProgressBar();
            this.circularProgressBar2 = new CircularProgressBar.CircularProgressBar();
            this.ProgressBarsContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // StartPomodoro
            // 
            this.StartPomodoro.Location = new System.Drawing.Point(12, 12);
            this.StartPomodoro.Name = "StartPomodoro";
            this.StartPomodoro.Size = new System.Drawing.Size(75, 23);
            this.StartPomodoro.TabIndex = 2;
            this.StartPomodoro.Text = "&Start";
            this.StartPomodoro.UseVisualStyleBackColor = true;
            this.StartPomodoro.Click += new System.EventHandler(this.StartPomodoroClickHandler);
            // 
            // ProgressBarsContainer
            // 
            this.ProgressBarsContainer.Controls.Add(this.Pomodoro1ProgressBar);
            this.ProgressBarsContainer.Controls.Add(this.Pomodoro2ProgressBar);
            this.ProgressBarsContainer.Controls.Add(this.circularProgressBar1);
            this.ProgressBarsContainer.Controls.Add(this.circularProgressBar2);
            this.ProgressBarsContainer.Location = new System.Drawing.Point(93, 0);
            this.ProgressBarsContainer.Name = "ProgressBarsContainer";
            this.ProgressBarsContainer.Size = new System.Drawing.Size(504, 126);
            this.ProgressBarsContainer.TabIndex = 3;
            // 
            // Pomodoro1ProgressBar
            // 
            this.Pomodoro1ProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn;
            this.Pomodoro1ProgressBar.AnimationSpeed = 500;
            this.Pomodoro1ProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.Pomodoro1ProgressBar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Pomodoro1ProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Pomodoro1ProgressBar.InnerColor = System.Drawing.SystemColors.Control;
            this.Pomodoro1ProgressBar.InnerMargin = 0;
            this.Pomodoro1ProgressBar.InnerWidth = 10;
            this.Pomodoro1ProgressBar.Location = new System.Drawing.Point(3, 3);
            this.Pomodoro1ProgressBar.MarqueeAnimationSpeed = 2000;
            this.Pomodoro1ProgressBar.Name = "Pomodoro1ProgressBar";
            this.Pomodoro1ProgressBar.OuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Pomodoro1ProgressBar.OuterMargin = -20;
            this.Pomodoro1ProgressBar.OuterWidth = 20;
            this.Pomodoro1ProgressBar.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.Pomodoro1ProgressBar.ProgressWidth = 20;
            this.Pomodoro1ProgressBar.SecondaryFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Pomodoro1ProgressBar.Size = new System.Drawing.Size(120, 120);
            this.Pomodoro1ProgressBar.StartAngle = 270;
            this.Pomodoro1ProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.Pomodoro1ProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.Pomodoro1ProgressBar.SubscriptText = "";
            this.Pomodoro1ProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.Pomodoro1ProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.Pomodoro1ProgressBar.SuperscriptText = "";
            this.Pomodoro1ProgressBar.TabIndex = 1;
            this.Pomodoro1ProgressBar.Text = "__:__ / __:__";
            this.Pomodoro1ProgressBar.TextMargin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.Pomodoro1ProgressBar.Value = 68;
            // 
            // Pomodoro2ProgressBar
            // 
            this.Pomodoro2ProgressBar.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn;
            this.Pomodoro2ProgressBar.AnimationSpeed = 500;
            this.Pomodoro2ProgressBar.BackColor = System.Drawing.Color.Transparent;
            this.Pomodoro2ProgressBar.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Pomodoro2ProgressBar.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Pomodoro2ProgressBar.InnerColor = System.Drawing.SystemColors.Control;
            this.Pomodoro2ProgressBar.InnerMargin = 0;
            this.Pomodoro2ProgressBar.InnerWidth = 10;
            this.Pomodoro2ProgressBar.Location = new System.Drawing.Point(129, 3);
            this.Pomodoro2ProgressBar.MarqueeAnimationSpeed = 2000;
            this.Pomodoro2ProgressBar.Name = "Pomodoro2ProgressBar";
            this.Pomodoro2ProgressBar.OuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Pomodoro2ProgressBar.OuterMargin = -20;
            this.Pomodoro2ProgressBar.OuterWidth = 20;
            this.Pomodoro2ProgressBar.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.Pomodoro2ProgressBar.ProgressWidth = 20;
            this.Pomodoro2ProgressBar.SecondaryFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Pomodoro2ProgressBar.Size = new System.Drawing.Size(120, 120);
            this.Pomodoro2ProgressBar.StartAngle = 270;
            this.Pomodoro2ProgressBar.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.Pomodoro2ProgressBar.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.Pomodoro2ProgressBar.SubscriptText = "";
            this.Pomodoro2ProgressBar.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.Pomodoro2ProgressBar.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.Pomodoro2ProgressBar.SuperscriptText = "";
            this.Pomodoro2ProgressBar.TabIndex = 2;
            this.Pomodoro2ProgressBar.Text = "__:__ / __:__";
            this.Pomodoro2ProgressBar.TextMargin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.Pomodoro2ProgressBar.Value = 68;
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn;
            this.circularProgressBar1.AnimationSpeed = 500;
            this.circularProgressBar1.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar1.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.circularProgressBar1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.circularProgressBar1.InnerColor = System.Drawing.SystemColors.Control;
            this.circularProgressBar1.InnerMargin = 0;
            this.circularProgressBar1.InnerWidth = 10;
            this.circularProgressBar1.Location = new System.Drawing.Point(255, 3);
            this.circularProgressBar1.MarqueeAnimationSpeed = 2000;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.OuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.circularProgressBar1.OuterMargin = -20;
            this.circularProgressBar1.OuterWidth = 20;
            this.circularProgressBar1.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.circularProgressBar1.ProgressWidth = 20;
            this.circularProgressBar1.SecondaryFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.circularProgressBar1.Size = new System.Drawing.Size(120, 120);
            this.circularProgressBar1.StartAngle = 270;
            this.circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar1.SubscriptText = "";
            this.circularProgressBar1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar1.SuperscriptText = "";
            this.circularProgressBar1.TabIndex = 3;
            this.circularProgressBar1.Text = "__:__ / __:__";
            this.circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.circularProgressBar1.Value = 68;
            // 
            // circularProgressBar2
            // 
            this.circularProgressBar2.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.QuinticEaseIn;
            this.circularProgressBar2.AnimationSpeed = 500;
            this.circularProgressBar2.BackColor = System.Drawing.Color.Transparent;
            this.circularProgressBar2.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.circularProgressBar2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.circularProgressBar2.InnerColor = System.Drawing.SystemColors.Control;
            this.circularProgressBar2.InnerMargin = 0;
            this.circularProgressBar2.InnerWidth = 10;
            this.circularProgressBar2.Location = new System.Drawing.Point(381, 3);
            this.circularProgressBar2.MarqueeAnimationSpeed = 2000;
            this.circularProgressBar2.Name = "circularProgressBar2";
            this.circularProgressBar2.OuterColor = System.Drawing.SystemColors.ControlDarkDark;
            this.circularProgressBar2.OuterMargin = -20;
            this.circularProgressBar2.OuterWidth = 20;
            this.circularProgressBar2.ProgressColor = System.Drawing.SystemColors.Highlight;
            this.circularProgressBar2.ProgressWidth = 20;
            this.circularProgressBar2.SecondaryFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.circularProgressBar2.Size = new System.Drawing.Size(120, 120);
            this.circularProgressBar2.StartAngle = 270;
            this.circularProgressBar2.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar2.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar2.SubscriptText = "";
            this.circularProgressBar2.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar2.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar2.SuperscriptText = "";
            this.circularProgressBar2.TabIndex = 4;
            this.circularProgressBar2.Text = "__:__ / __:__";
            this.circularProgressBar2.TextMargin = new System.Windows.Forms.Padding(0, 8, 0, 0);
            this.circularProgressBar2.Value = 68;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(599, 127);
            this.Controls.Add(this.ProgressBarsContainer);
            this.Controls.Add(this.StartPomodoro);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Pomodour";
            this.ProgressBarsContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private Button StartPomodoro;
        private FlowLayoutPanel ProgressBarsContainer;
        private CircularProgressBar.CircularProgressBar Pomodoro1ProgressBar;
        private CircularProgressBar.CircularProgressBar Pomodoro2ProgressBar;
        private CircularProgressBar.CircularProgressBar circularProgressBar1;
        private CircularProgressBar.CircularProgressBar circularProgressBar2;
    }
}