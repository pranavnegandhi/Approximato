﻿namespace Notadesigner.Approximato.Windows
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
            mainLayoutPanel = new TableLayoutPanel();
            progressBarsContainer = new FlowLayoutPanel();
            mainStatusStrip = new StatusStrip();
            currentStateStatusLabel = new ToolStripStatusLabel();
            currentPhaseStatusLabel = new ToolStripStatusLabel();
            mainLayoutPanel.SuspendLayout();
            mainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            mainLayoutPanel.ColumnCount = 1;
            mainLayoutPanel.ColumnStyles.Add(new ColumnStyle());
            mainLayoutPanel.Controls.Add(progressBarsContainer, 0, 0);
            mainLayoutPanel.Controls.Add(mainStatusStrip, 0, 1);
            mainLayoutPanel.Dock = DockStyle.Fill;
            mainLayoutPanel.Location = new Point(0, 0);
            mainLayoutPanel.Name = "mainLayoutPanel";
            mainLayoutPanel.RowCount = 2;
            mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            mainLayoutPanel.Size = new Size(236, 101);
            mainLayoutPanel.TabIndex = 4;
            // 
            // progressBarsContainer
            // 
            progressBarsContainer.Dock = DockStyle.Fill;
            progressBarsContainer.Location = new Point(3, 3);
            progressBarsContainer.Name = "progressBarsContainer";
            progressBarsContainer.Size = new Size(236, 101);
            progressBarsContainer.TabIndex = 4;
            // 
            // mainStatusStrip
            // 
            mainStatusStrip.Items.AddRange(new ToolStripItem[] { currentStateStatusLabel, currentPhaseStatusLabel });
            mainStatusStrip.Location = new Point(0, 79);
            mainStatusStrip.Name = "mainStatusStrip";
            mainStatusStrip.Size = new Size(236, 22);
            mainStatusStrip.SizingGrip = false;
            mainStatusStrip.TabIndex = 1;
            mainStatusStrip.Text = "mainStatusStrip";
            // 
            // currentStateStatusLabel
            // 
            currentStateStatusLabel.ImageScaling = ToolStripItemImageScaling.None;
            currentStateStatusLabel.Name = "currentStateStatusLabel";
            currentStateStatusLabel.Size = new Size(0, 17);
            // 
            // currentPhaseStatusLabel
            // 
            currentPhaseStatusLabel.Name = "currentPhaseStatusLabel";
            currentPhaseStatusLabel.Size = new Size(0, 17);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(236, 101);
            Controls.Add(mainLayoutPanel);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            ShowInTaskbar = false;
            SizeGripStyle = SizeGripStyle.Hide;
            TopMost = true;
            mainLayoutPanel.ResumeLayout(false);
            mainLayoutPanel.PerformLayout();
            mainStatusStrip.ResumeLayout(false);
            mainStatusStrip.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel mainLayoutPanel;
        private FlowLayoutPanel progressBarsContainer;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel currentStateStatusLabel;
        private ToolStripStatusLabel currentPhaseStatusLabel;
    }
}