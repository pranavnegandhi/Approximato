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
            MainContainer = new TableLayoutPanel();
            CloseSettings = new Button();
            AboutInfoContainer = new GroupBox();
            AboutInfoLabel = new Label();
            AboutInfoImage = new PictureBox();
            flowLayoutPanel1 = new FlowLayoutPanel();
            EnableLenientMode = new CheckBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            MaximumRoundsLabel = new Label();
            MaximumRoundsInput = new ComboBox();
            groupBox1 = new GroupBox();
            flowLayoutPanel6 = new FlowLayoutPanel();
            flowLayoutPanel3 = new FlowLayoutPanel();
            WorkSessionLabel = new Label();
            WorkSessionInput = new NumericUpDown();
            flowLayoutPanel4 = new FlowLayoutPanel();
            ShortBreakLabel = new Label();
            ShortBreakInput = new NumericUpDown();
            flowLayoutPanel5 = new FlowLayoutPanel();
            LongBreakLabel = new Label();
            LongBreakInput = new NumericUpDown();
            InstructionsLabel = new Label();
            MainContainer.SuspendLayout();
            AboutInfoContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)AboutInfoImage).BeginInit();
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel2.SuspendLayout();
            groupBox1.SuspendLayout();
            flowLayoutPanel6.SuspendLayout();
            flowLayoutPanel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)WorkSessionInput).BeginInit();
            flowLayoutPanel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ShortBreakInput).BeginInit();
            flowLayoutPanel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)LongBreakInput).BeginInit();
            SuspendLayout();
            // 
            // MainContainer
            // 
            MainContainer.ColumnCount = 2;
            MainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            MainContainer.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            MainContainer.Controls.Add(CloseSettings, 1, 1);
            MainContainer.Controls.Add(AboutInfoContainer, 0, 0);
            MainContainer.Controls.Add(flowLayoutPanel1, 1, 0);
            MainContainer.Dock = DockStyle.Fill;
            MainContainer.Location = new Point(0, 0);
            MainContainer.Name = "MainContainer";
            MainContainer.RowCount = 2;
            MainContainer.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            MainContainer.RowStyles.Add(new RowStyle());
            MainContainer.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            MainContainer.Size = new Size(484, 311);
            MainContainer.TabIndex = 0;
            // 
            // CloseSettings
            // 
            CloseSettings.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            CloseSettings.Location = new Point(406, 285);
            CloseSettings.Name = "CloseSettings";
            CloseSettings.Size = new Size(75, 23);
            CloseSettings.TabIndex = 4;
            CloseSettings.Text = "&Close";
            CloseSettings.UseVisualStyleBackColor = true;
            // 
            // AboutInfoContainer
            // 
            AboutInfoContainer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            AboutInfoContainer.Controls.Add(AboutInfoLabel);
            AboutInfoContainer.Controls.Add(AboutInfoImage);
            AboutInfoContainer.Location = new Point(10, 10);
            AboutInfoContainer.Margin = new Padding(10);
            AboutInfoContainer.Name = "AboutInfoContainer";
            AboutInfoContainer.Size = new Size(222, 262);
            AboutInfoContainer.TabIndex = 0;
            AboutInfoContainer.TabStop = false;
            AboutInfoContainer.Text = "About";
            // 
            // AboutInfoLabel
            // 
            AboutInfoLabel.Location = new Point(6, 80);
            AboutInfoLabel.Name = "AboutInfoLabel";
            AboutInfoLabel.Size = new Size(210, 161);
            AboutInfoLabel.TabIndex = 2;
            AboutInfoLabel.Text = "label1";
            AboutInfoLabel.TextAlign = ContentAlignment.TopCenter;
            // 
            // AboutInfoImage
            // 
            AboutInfoImage.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            AboutInfoImage.BackgroundImage = Properties.GuiRunnerResources.AppLogo_32_48;
            AboutInfoImage.BackgroundImageLayout = ImageLayout.Center;
            AboutInfoImage.Location = new Point(6, 25);
            AboutInfoImage.Name = "AboutInfoImage";
            AboutInfoImage.Size = new Size(210, 50);
            AboutInfoImage.TabIndex = 0;
            AboutInfoImage.TabStop = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            flowLayoutPanel1.Controls.Add(EnableLenientMode);
            flowLayoutPanel1.Controls.Add(flowLayoutPanel2);
            flowLayoutPanel1.Controls.Add(groupBox1);
            flowLayoutPanel1.Controls.Add(InstructionsLabel);
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(252, 10);
            flowLayoutPanel1.Margin = new Padding(10);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(3);
            flowLayoutPanel1.Size = new Size(222, 262);
            flowLayoutPanel1.TabIndex = 1;
            // 
            // EnableLenientMode
            // 
            EnableLenientMode.AutoSize = true;
            EnableLenientMode.Location = new Point(6, 6);
            EnableLenientMode.Name = "EnableLenientMode";
            EnableLenientMode.Size = new Size(99, 19);
            EnableLenientMode.TabIndex = 0;
            EnableLenientMode.Text = "&Lenient Mode";
            EnableLenientMode.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.AutoSize = true;
            flowLayoutPanel2.Controls.Add(MaximumRoundsLabel);
            flowLayoutPanel2.Controls.Add(MaximumRoundsInput);
            flowLayoutPanel2.Location = new Point(6, 31);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(182, 29);
            flowLayoutPanel2.TabIndex = 0;
            flowLayoutPanel2.WrapContents = false;
            // 
            // MaximumRoundsLabel
            // 
            MaximumRoundsLabel.Anchor = AnchorStyles.Left;
            MaximumRoundsLabel.Location = new Point(3, 7);
            MaximumRoundsLabel.Name = "MaximumRoundsLabel";
            MaximumRoundsLabel.Size = new Size(90, 15);
            MaximumRoundsLabel.TabIndex = 0;
            MaximumRoundsLabel.Text = "&Rounds";
            // 
            // MaximumRoundsInput
            // 
            MaximumRoundsInput.DropDownStyle = ComboBoxStyle.DropDownList;
            MaximumRoundsInput.FormattingEnabled = true;
            MaximumRoundsInput.Location = new Point(99, 3);
            MaximumRoundsInput.Name = "MaximumRoundsInput";
            MaximumRoundsInput.Size = new Size(80, 23);
            MaximumRoundsInput.TabIndex = 6;
            // 
            // groupBox1
            // 
            groupBox1.AutoSize = true;
            groupBox1.Controls.Add(flowLayoutPanel6);
            groupBox1.Location = new Point(6, 66);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(209, 146);
            groupBox1.TabIndex = 4;
            groupBox1.TabStop = false;
            groupBox1.Text = "Duration in Minutes";
            // 
            // flowLayoutPanel6
            // 
            flowLayoutPanel6.AutoSize = true;
            flowLayoutPanel6.Controls.Add(flowLayoutPanel3);
            flowLayoutPanel6.Controls.Add(flowLayoutPanel4);
            flowLayoutPanel6.Controls.Add(flowLayoutPanel5);
            flowLayoutPanel6.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel6.Location = new Point(3, 19);
            flowLayoutPanel6.Name = "flowLayoutPanel6";
            flowLayoutPanel6.Size = new Size(200, 105);
            flowLayoutPanel6.TabIndex = 0;
            // 
            // flowLayoutPanel3
            // 
            flowLayoutPanel3.AutoSize = true;
            flowLayoutPanel3.Controls.Add(WorkSessionLabel);
            flowLayoutPanel3.Controls.Add(WorkSessionInput);
            flowLayoutPanel3.Location = new Point(3, 3);
            flowLayoutPanel3.Name = "flowLayoutPanel3";
            flowLayoutPanel3.Size = new Size(182, 29);
            flowLayoutPanel3.TabIndex = 4;
            flowLayoutPanel3.WrapContents = false;
            // 
            // WorkSessionLabel
            // 
            WorkSessionLabel.Anchor = AnchorStyles.Left;
            WorkSessionLabel.Location = new Point(3, 7);
            WorkSessionLabel.Name = "WorkSessionLabel";
            WorkSessionLabel.Size = new Size(90, 15);
            WorkSessionLabel.TabIndex = 0;
            WorkSessionLabel.Text = "&Focus";
            // 
            // WorkSessionInput
            // 
            WorkSessionInput.Anchor = AnchorStyles.None;
            WorkSessionInput.Location = new Point(99, 3);
            WorkSessionInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            WorkSessionInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            WorkSessionInput.Name = "WorkSessionInput";
            WorkSessionInput.Size = new Size(80, 23);
            WorkSessionInput.TabIndex = 11;
            WorkSessionInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // flowLayoutPanel4
            // 
            flowLayoutPanel4.AutoSize = true;
            flowLayoutPanel4.Controls.Add(ShortBreakLabel);
            flowLayoutPanel4.Controls.Add(ShortBreakInput);
            flowLayoutPanel4.Location = new Point(3, 38);
            flowLayoutPanel4.Name = "flowLayoutPanel4";
            flowLayoutPanel4.Size = new Size(182, 29);
            flowLayoutPanel4.TabIndex = 5;
            flowLayoutPanel4.WrapContents = false;
            // 
            // ShortBreakLabel
            // 
            ShortBreakLabel.Anchor = AnchorStyles.Left;
            ShortBreakLabel.Location = new Point(3, 7);
            ShortBreakLabel.Name = "ShortBreakLabel";
            ShortBreakLabel.Size = new Size(90, 15);
            ShortBreakLabel.TabIndex = 0;
            ShortBreakLabel.Text = "&Short Break";
            // 
            // ShortBreakInput
            // 
            ShortBreakInput.Anchor = AnchorStyles.None;
            ShortBreakInput.Location = new Point(99, 3);
            ShortBreakInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            ShortBreakInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            ShortBreakInput.Name = "ShortBreakInput";
            ShortBreakInput.Size = new Size(80, 23);
            ShortBreakInput.TabIndex = 11;
            ShortBreakInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // flowLayoutPanel5
            // 
            flowLayoutPanel5.AutoSize = true;
            flowLayoutPanel5.Controls.Add(LongBreakLabel);
            flowLayoutPanel5.Controls.Add(LongBreakInput);
            flowLayoutPanel5.Location = new Point(3, 73);
            flowLayoutPanel5.Name = "flowLayoutPanel5";
            flowLayoutPanel5.Size = new Size(182, 29);
            flowLayoutPanel5.TabIndex = 6;
            flowLayoutPanel5.WrapContents = false;
            // 
            // LongBreakLabel
            // 
            LongBreakLabel.Anchor = AnchorStyles.Left;
            LongBreakLabel.Location = new Point(3, 7);
            LongBreakLabel.Name = "LongBreakLabel";
            LongBreakLabel.Size = new Size(90, 15);
            LongBreakLabel.TabIndex = 0;
            LongBreakLabel.Text = "&Long Break";
            // 
            // LongBreakInput
            // 
            LongBreakInput.Anchor = AnchorStyles.None;
            LongBreakInput.Location = new Point(99, 3);
            LongBreakInput.Maximum = new decimal(new int[] { 60, 0, 0, 0 });
            LongBreakInput.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            LongBreakInput.Name = "LongBreakInput";
            LongBreakInput.Size = new Size(80, 23);
            LongBreakInput.TabIndex = 11;
            LongBreakInput.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // InstructionsLabel
            // 
            InstructionsLabel.AutoSize = true;
            InstructionsLabel.Location = new Point(3, 215);
            InstructionsLabel.Margin = new Padding(0, 0, 0, 6);
            InstructionsLabel.Name = "InstructionsLabel";
            InstructionsLabel.Size = new Size(183, 15);
            InstructionsLabel.TabIndex = 6;
            InstructionsLabel.Text = "Changes are saved automatically.";
            // 
            // SettingsForm
            // 
            AcceptButton = CloseSettings;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            CancelButton = CloseSettings;
            ClientSize = new Size(484, 311);
            ControlBox = false;
            Controls.Add(MainContainer);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Name = "SettingsForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Settings";
            MainContainer.ResumeLayout(false);
            AboutInfoContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)AboutInfoImage).EndInit();
            flowLayoutPanel1.ResumeLayout(false);
            flowLayoutPanel1.PerformLayout();
            flowLayoutPanel2.ResumeLayout(false);
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            flowLayoutPanel6.ResumeLayout(false);
            flowLayoutPanel6.PerformLayout();
            flowLayoutPanel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)WorkSessionInput).EndInit();
            flowLayoutPanel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ShortBreakInput).EndInit();
            flowLayoutPanel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)LongBreakInput).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel MainContainer;
        private GroupBox AboutInfoContainer;
        private FlowLayoutPanel flowLayoutPanel1;
        private CheckBox EnableLenientMode;
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
        private FlowLayoutPanel flowLayoutPanel2;
        private Label MaximumRoundsLabel;
        private ComboBox MaximumRoundsInput;
    }
}