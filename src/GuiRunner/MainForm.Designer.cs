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
            this.ProgressBarsContainer.Location = new System.Drawing.Point(93, 0);
            this.ProgressBarsContainer.Name = "ProgressBarsContainer";
            this.ProgressBarsContainer.Size = new System.Drawing.Size(504, 126);
            this.ProgressBarsContainer.TabIndex = 3;
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
    }
}