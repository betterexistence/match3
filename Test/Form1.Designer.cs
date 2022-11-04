
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
            this.scoreLabel = new System.Windows.Forms.Label();
            this.startGameButton = new System.Windows.Forms.Button();
            this.stopGameButton = new System.Windows.Forms.Button();
            this.timerLabel = new System.Windows.Forms.Label();
            this.threeinarow1 = new threeInARow.ThreeInARow();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scoreLabel.Location = new System.Drawing.Point(449, 12);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(148, 25);
            this.scoreLabel.TabIndex = 2;
            this.scoreLabel.Text = "Текущий счет:";
            // 
            // startGameButton
            // 
            this.startGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startGameButton.Location = new System.Drawing.Point(449, 360);
            this.startGameButton.Name = "startGameButton";
            this.startGameButton.Size = new System.Drawing.Size(173, 35);
            this.startGameButton.TabIndex = 3;
            this.startGameButton.Text = "Start";
            this.startGameButton.UseVisualStyleBackColor = true;
            this.startGameButton.Click += new System.EventHandler(this.startGameButton_Click);
            // 
            // stopGameButton
            // 
            this.stopGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.stopGameButton.Location = new System.Drawing.Point(449, 401);
            this.stopGameButton.Name = "stopGameButton";
            this.stopGameButton.Size = new System.Drawing.Size(173, 35);
            this.stopGameButton.TabIndex = 4;
            this.stopGameButton.Text = "Stop";
            this.stopGameButton.UseVisualStyleBackColor = true;
            this.stopGameButton.Click += new System.EventHandler(this.stopGameButton_Click);
            // 
            // timerLabel
            // 
            this.timerLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timerLabel.AutoSize = true;
            this.timerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.timerLabel.Location = new System.Drawing.Point(449, 55);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(90, 25);
            this.timerLabel.TabIndex = 5;
            this.timerLabel.Text = "Таймер:";
            // 
            // threeinarow1
            // 
            this.threeinarow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threeinarow1.BackColor = System.Drawing.SystemColors.Control;
            this.threeinarow1.ColorBackground = System.Drawing.Color.SkyBlue;
            this.threeinarow1.Location = new System.Drawing.Point(12, 12);
            this.threeinarow1.Margin = new System.Windows.Forms.Padding(10);
            this.threeinarow1.Name = "threeinarow1";
            this.threeinarow1.Size = new System.Drawing.Size(424, 424);
            this.threeinarow1.TabIndex = 1;
            this.threeinarow1.Text = "threeinarow1";
            this.threeinarow1.TimerCount = 70;
            this.threeinarow1.EventScore += new System.EventHandler(this.scoreLabel_eventScore);
            this.threeinarow1.EventTimer += new System.EventHandler(this.timerLabel_eventTimer);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(634, 451);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.stopGameButton);
            this.Controls.Add(this.startGameButton);
            this.Controls.Add(this.scoreLabel);
            this.Controls.Add(this.threeinarow1);
            this.MinimumSize = new System.Drawing.Size(650, 490);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private threeInARow.ThreeInARow threeinarow1;
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Button startGameButton;
        private System.Windows.Forms.Button stopGameButton;
        private System.Windows.Forms.Label timerLabel;
    }
}

