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
            this.StartBreak = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // StartPomodoro
            // 
            this.StartPomodoro.Location = new System.Drawing.Point(12, 12);
            this.StartPomodoro.Name = "StartPomodoro";
            this.StartPomodoro.Size = new System.Drawing.Size(135, 23);
            this.StartPomodoro.TabIndex = 0;
            this.StartPomodoro.Text = "&Start Pomodoro";
            this.StartPomodoro.UseVisualStyleBackColor = true;
            this.StartPomodoro.Click += new System.EventHandler(this.StartPomodoroClickHandler);
            // 
            // StartBreak
            // 
            this.StartBreak.Enabled = false;
            this.StartBreak.Location = new System.Drawing.Point(12, 41);
            this.StartBreak.Name = "StartBreak";
            this.StartBreak.Size = new System.Drawing.Size(135, 23);
            this.StartBreak.TabIndex = 1;
            this.StartBreak.Text = "Start &Break";
            this.StartBreak.UseVisualStyleBackColor = true;
            this.StartBreak.Click += new System.EventHandler(this.StartBreakClickHandler);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.StartBreak);
            this.Controls.Add(this.StartPomodoro);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button StartPomodoro;
        private Button StartBreak;
    }
}