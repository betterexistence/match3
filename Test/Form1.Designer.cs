
namespace Test
{
    partial class Form1
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
            this.threeinarow1 = new threeInARow.ThreeInARow();
            this.SuspendLayout();
            // 
            // threeinarow1
            // 
            this.threeinarow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threeinarow1.Location = new System.Drawing.Point(12, 12);
            this.threeinarow1.Name = "threeinarow1";
            this.threeinarow1.Score = 0;
            this.threeinarow1.Size = new System.Drawing.Size(424, 424);
            this.threeinarow1.TabIndex = 1;
            this.threeinarow1.Text = "threeinarow1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(454, 451);
            this.Controls.Add(this.threeinarow1);
            this.MinimumSize = new System.Drawing.Size(470, 490);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private threeInARow.ThreeInARow threeinarow1;
    }
}

