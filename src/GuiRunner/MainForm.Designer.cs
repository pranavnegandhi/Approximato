namespace Notadesigner.Tom.App
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
            this.mainLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.progressBarsContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.currentPhaseStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.mainLayoutPanel.SuspendLayout();
            this.mainStatusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // mainLayoutPanel
            // 
            this.mainLayoutPanel.ColumnCount = 1;
            this.mainLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.mainLayoutPanel.Controls.Add(this.progressBarsContainer, 0, 0);
            this.mainLayoutPanel.Controls.Add(this.mainStatusStrip, 0, 1);
            this.mainLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.mainLayoutPanel.Name = "mainLayoutPanel";
            this.mainLayoutPanel.RowCount = 2;
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.mainLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            this.mainLayoutPanel.Size = new System.Drawing.Size(236, 151);
            this.mainLayoutPanel.TabIndex = 4;
            // 
            // progressBarsContainer
            // 
            this.progressBarsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressBarsContainer.Location = new System.Drawing.Point(3, 3);
            this.progressBarsContainer.Name = "progressBarsContainer";
            this.progressBarsContainer.Size = new System.Drawing.Size(236, 123);
            this.progressBarsContainer.TabIndex = 4;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.currentPhaseStatusLabel});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 129);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(242, 22);
            this.mainStatusStrip.SizingGrip = false;
            this.mainStatusStrip.TabIndex = 1;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // currentPhaseStatusLabel
            // 
            this.currentPhaseStatusLabel.Name = "currentPhaseStatusLabel";
            this.currentPhaseStatusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(236, 151);
            this.Controls.Add(this.mainLayoutPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Tom";
            this.mainLayoutPanel.ResumeLayout(false);
            this.mainLayoutPanel.PerformLayout();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private TableLayoutPanel mainLayoutPanel;
        private FlowLayoutPanel progressBarsContainer;
        private StatusStrip mainStatusStrip;
        private ToolStripStatusLabel currentPhaseStatusLabel;
    }
}