namespace Prog225FinalProject___Victor_Besson
{
    partial class ArenaForm
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
            this.pnInfo = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // pnInfo
            // 
            this.pnInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.pnInfo.Location = new System.Drawing.Point(0, 0);
            this.pnInfo.MaximumSize = new System.Drawing.Size(1049, 109);
            this.pnInfo.MinimumSize = new System.Drawing.Size(1049, 109);
            this.pnInfo.Name = "pnInfo";
            this.pnInfo.Size = new System.Drawing.Size(1049, 109);
            this.pnInfo.TabIndex = 0;
            // 
            // ArenaForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1049, 778);
            this.Controls.Add(this.pnInfo);
            this.MaximumSize = new System.Drawing.Size(1065, 817);
            this.MinimumSize = new System.Drawing.Size(1065, 817);
            this.Name = "ArenaForm";
            this.Text = "Battle Cycles";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnInfo;
    }
}