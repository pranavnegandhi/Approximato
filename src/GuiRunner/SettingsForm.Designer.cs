namespace Notadesigner.Tom.App
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MainContainer = new System.Windows.Forms.TableLayoutPanel();
            this.CloseSettings = new System.Windows.Forms.Button();
            this.AboutInfoContainer = new System.Windows.Forms.GroupBox();
            this.AboutInfoLabel = new System.Windows.Forms.Label();
            this.AboutInfoImage = new System.Windows.Forms.PictureBox();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.InstructionsLabel = new System.Windows.Forms.Label();
            this.EnableAutoAdvance = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.flowLayoutPanel6 = new System.Windows.Forms.FlowLayoutPanel();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.WorkSessionLabel = new System.Windows.Forms.Label();
            this.WorkSessionInput = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel4 = new System.Windows.Forms.FlowLayoutPanel();
            this.ShortBreakLabel = new System.Windows.Forms.Label();
            this.ShortBreakInput = new System.Windows.Forms.NumericUpDown();
            this.flowLayoutPanel5 = new System.Windows.Forms.FlowLayoutPanel();
            this.LongBreakLabel = new System.Windows.Forms.Label();
            this.LongBreakInput = new System.Windows.Forms.NumericUpDown();
            this.MainContainer.SuspendLayout();
            this.AboutInfoContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutInfoImage)).BeginInit();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.flowLayoutPanel6.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.WorkSessionInput)).BeginInit();
            this.flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ShortBreakInput)).BeginInit();
            this.flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.LongBreakInput)).BeginInit();
            this.SuspendLayout();
            // 
            // MainContainer
            // 
            this.MainContainer.ColumnCount = 2;
            this.MainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainContainer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.MainContainer.Controls.Add(this.CloseSettings, 1, 1);
            this.MainContainer.Controls.Add(this.AboutInfoContainer, 0, 0);
            this.MainContainer.Controls.Add(this.flowLayoutPanel1, 1, 0);
            this.MainContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainContainer.Location = new System.Drawing.Point(0, 0);
            this.MainContainer.Name = "MainContainer";
            this.MainContainer.RowCount = 2;
            this.MainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.MainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.MainContainer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.MainContainer.Size = new System.Drawing.Size(484, 311);
            this.MainContainer.TabIndex = 0;
            // 
            // CloseSettings
            // 
            this.CloseSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.CloseSettings.Location = new System.Drawing.Point(406, 285);
            this.CloseSettings.Name = "CloseSettings";
            this.CloseSettings.Size = new System.Drawing.Size(75, 23);
            this.CloseSettings.TabIndex = 4;
            this.CloseSettings.Text = "&Close";
            this.CloseSettings.UseVisualStyleBackColor = true;
            // 
            // AboutInfoContainer
            // 
            this.AboutInfoContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutInfoContainer.Controls.Add(this.AboutInfoLabel);
            this.AboutInfoContainer.Controls.Add(this.AboutInfoImage);
            this.AboutInfoContainer.Location = new System.Drawing.Point(10, 10);
            this.AboutInfoContainer.Margin = new System.Windows.Forms.Padding(10);
            this.AboutInfoContainer.Name = "AboutInfoContainer";
            this.AboutInfoContainer.Size = new System.Drawing.Size(222, 262);
            this.AboutInfoContainer.TabIndex = 0;
            this.AboutInfoContainer.TabStop = false;
            this.AboutInfoContainer.Text = "About";
            // 
            // AboutInfoLabel
            // 
            this.AboutInfoLabel.Location = new System.Drawing.Point(6, 80);
            this.AboutInfoLabel.Name = "AboutInfoLabel";
            this.AboutInfoLabel.Size = new System.Drawing.Size(210, 161);
            this.AboutInfoLabel.TabIndex = 2;
            this.AboutInfoLabel.Text = "label1";
            this.AboutInfoLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // AboutInfoImage
            // 
            this.AboutInfoImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.AboutInfoImage.BackgroundImage = global::Notadesigner.Tom.App.Properties.GuiRunnerResources.AppLogo_32_48;
            this.AboutInfoImage.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.AboutInfoImage.Location = new System.Drawing.Point(6, 25);
            this.AboutInfoImage.Name = "AboutInfoImage";
            this.AboutInfoImage.Size = new System.Drawing.Size(210, 50);
            this.AboutInfoImage.TabIndex = 0;
            this.AboutInfoImage.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.flowLayoutPanel1.Controls.Add(this.InstructionsLabel);
            this.flowLayoutPanel1.Controls.Add(this.EnableAutoAdvance);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(252, 10);
            this.flowLayoutPanel1.Margin = new System.Windows.Forms.Padding(10);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Padding = new System.Windows.Forms.Padding(3);
            this.flowLayoutPanel1.Size = new System.Drawing.Size(222, 262);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // InstructionsLabel
            // 
            this.InstructionsLabel.AutoSize = true;
            this.InstructionsLabel.Location = new System.Drawing.Point(3, 3);
            this.InstructionsLabel.Margin = new System.Windows.Forms.Padding(0, 0, 0, 6);
            this.InstructionsLabel.Name = "InstructionsLabel";
            this.InstructionsLabel.Size = new System.Drawing.Size(183, 15);
            this.InstructionsLabel.TabIndex = 6;
            this.InstructionsLabel.Text = "Changes are saved automatically.";
            // 
            // EnableAutoAdvance
            // 
            this.EnableAutoAdvance.AutoSize = true;
            this.EnableAutoAdvance.Enabled = false;
            this.EnableAutoAdvance.Location = new System.Drawing.Point(6, 27);
            this.EnableAutoAdvance.Name = "EnableAutoAdvance";
            this.EnableAutoAdvance.Size = new System.Drawing.Size(191, 19);
            this.EnableAutoAdvance.TabIndex = 0;
            this.EnableAutoAdvance.Text = "Auto-ad&vance to the next stage";
            this.EnableAutoAdvance.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSize = true;
            this.groupBox1.Controls.Add(this.flowLayoutPanel6);
            this.groupBox1.Location = new System.Drawing.Point(6, 52);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(209, 146);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Duration in Minutes";
            // 
            // flowLayoutPanel6
            // 
            this.flowLayoutPanel6.AutoSize = true;
            this.flowLayoutPanel6.Controls.Add(this.flowLayoutPanel3);
            this.flowLayoutPanel6.Controls.Add(this.flowLayoutPanel4);
            this.flowLayoutPanel6.Controls.Add(this.flowLayoutPanel5);
            this.flowLayoutPanel6.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowLayoutPanel6.Location = new System.Drawing.Point(3, 19);
            this.flowLayoutPanel6.Name = "flowLayoutPanel6";
            this.flowLayoutPanel6.Size = new System.Drawing.Size(200, 105);
            this.flowLayoutPanel6.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.AutoSize = true;
            this.flowLayoutPanel3.Controls.Add(this.WorkSessionLabel);
            this.flowLayoutPanel3.Controls.Add(this.WorkSessionInput);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(182, 29);
            this.flowLayoutPanel3.TabIndex = 4;
            this.flowLayoutPanel3.WrapContents = false;
            // 
            // WorkSessionLabel
            // 
            this.WorkSessionLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.WorkSessionLabel.Location = new System.Drawing.Point(3, 7);
            this.WorkSessionLabel.Name = "WorkSessionLabel";
            this.WorkSessionLabel.Size = new System.Drawing.Size(90, 15);
            this.WorkSessionLabel.TabIndex = 0;
            this.WorkSessionLabel.Text = "&Work Session";
            // 
            // WorkSessionInput
            // 
            this.WorkSessionInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.WorkSessionInput.Location = new System.Drawing.Point(99, 3);
            this.WorkSessionInput.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.WorkSessionInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.WorkSessionInput.Name = "WorkSessionInput";
            this.WorkSessionInput.Size = new System.Drawing.Size(80, 23);
            this.WorkSessionInput.TabIndex = 1;
            this.WorkSessionInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // flowLayoutPanel4
            // 
            this.flowLayoutPanel4.AutoSize = true;
            this.flowLayoutPanel4.Controls.Add(this.ShortBreakLabel);
            this.flowLayoutPanel4.Controls.Add(this.ShortBreakInput);
            this.flowLayoutPanel4.Location = new System.Drawing.Point(3, 38);
            this.flowLayoutPanel4.Name = "flowLayoutPanel4";
            this.flowLayoutPanel4.Size = new System.Drawing.Size(182, 29);
            this.flowLayoutPanel4.TabIndex = 5;
            this.flowLayoutPanel4.WrapContents = false;
            // 
            // ShortBreakLabel
            // 
            this.ShortBreakLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.ShortBreakLabel.Location = new System.Drawing.Point(3, 7);
            this.ShortBreakLabel.Name = "ShortBreakLabel";
            this.ShortBreakLabel.Size = new System.Drawing.Size(90, 15);
            this.ShortBreakLabel.TabIndex = 0;
            this.ShortBreakLabel.Text = "&Short Break";
            // 
            // ShortBreakInput
            // 
            this.ShortBreakInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ShortBreakInput.Location = new System.Drawing.Point(99, 3);
            this.ShortBreakInput.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.ShortBreakInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ShortBreakInput.Name = "ShortBreakInput";
            this.ShortBreakInput.Size = new System.Drawing.Size(80, 23);
            this.ShortBreakInput.TabIndex = 1;
            this.ShortBreakInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // flowLayoutPanel5
            // 
            this.flowLayoutPanel5.AutoSize = true;
            this.flowLayoutPanel5.Controls.Add(this.LongBreakLabel);
            this.flowLayoutPanel5.Controls.Add(this.LongBreakInput);
            this.flowLayoutPanel5.Location = new System.Drawing.Point(3, 73);
            this.flowLayoutPanel5.Name = "flowLayoutPanel5";
            this.flowLayoutPanel5.Size = new System.Drawing.Size(182, 29);
            this.flowLayoutPanel5.TabIndex = 6;
            this.flowLayoutPanel5.WrapContents = false;
            // 
            // LongBreakLabel
            // 
            this.LongBreakLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.LongBreakLabel.Location = new System.Drawing.Point(3, 7);
            this.LongBreakLabel.Name = "LongBreakLabel";
            this.LongBreakLabel.Size = new System.Drawing.Size(90, 15);
            this.LongBreakLabel.TabIndex = 0;
            this.LongBreakLabel.Text = "&Long Break";
            // 
            // LongBreakInput
            // 
            this.LongBreakInput.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.LongBreakInput.Location = new System.Drawing.Point(99, 3);
            this.LongBreakInput.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.LongBreakInput.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.LongBreakInput.Name = "LongBreakInput";
            this.LongBreakInput.Size = new System.Drawing.Size(80, 23);
            this.LongBreakInput.TabIndex = 1;
            this.LongBreakInput.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.CloseSettings;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.CloseSettings;
            this.ClientSize = new System.Drawing.Size(484, 311);
            this.ControlBox = false;
            this.Controls.Add(this.MainContainer);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.MainContainer.ResumeLayout(false);
            this.AboutInfoContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AboutInfoImage)).EndInit();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.flowLayoutPanel6.ResumeLayout(false);
            this.flowLayoutPanel6.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.WorkSessionInput)).EndInit();
            this.flowLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ShortBreakInput)).EndInit();
            this.flowLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.LongBreakInput)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private TableLayoutPanel MainContainer;
        private GroupBox AboutInfoContainer;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox EnableAutoAdvance;
        private PictureBox AboutInfoImage;
        private Label AboutInfoLabel;
        private GroupBox groupBox1;
        private FlowLayoutPanel flowLayoutPanel6;
        private FlowLayoutPanel flowLayoutPanel3;
        private Label WorkSessionLabel;
        private NumericUpDown WorkSessionInput;
        private FlowLayoutPanel flowLayoutPanel4;
        private Label ShortBreakLabel;
        private NumericUpDown ShortBreakInput;
        private FlowLayoutPanel flowLayoutPanel5;
        private Label LongBreakLabel;
        private NumericUpDown LongBreakInput;
        private Button CloseSettings;
        private Label InstructionsLabel;
    }
}