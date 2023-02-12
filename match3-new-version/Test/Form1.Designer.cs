
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
            this.threeInARow1 = new threeInARow.ThreeInARow();
            this.SuspendLayout();
            // 
            // scoreLabel
            // 
            this.scoreLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scoreLabel.AutoSize = true;
            this.scoreLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.scoreLabel.Location = new System.Drawing.Point(657, 12);
            this.scoreLabel.Name = "scoreLabel";
            this.scoreLabel.Size = new System.Drawing.Size(148, 25);
            this.scoreLabel.TabIndex = 2;
            this.scoreLabel.Text = "Текущий счет:";
            // 
            // startGameButton
            // 
            this.startGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.startGameButton.Location = new System.Drawing.Point(689, 539);
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
            this.stopGameButton.Location = new System.Drawing.Point(689, 580);
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
            this.timerLabel.Location = new System.Drawing.Point(657, 55);
            this.timerLabel.Name = "timerLabel";
            this.timerLabel.Size = new System.Drawing.Size(90, 25);
            this.timerLabel.TabIndex = 5;
            this.timerLabel.Text = "Таймер:";
            // 
            // threeInARow1
            // 
            this.threeInARow1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.threeInARow1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.threeInARow1.ColorBackground = System.Drawing.Color.Maroon;
            this.threeInARow1.Location = new System.Drawing.Point(13, 12);
            this.threeInARow1.Name = "threeInARow1";
            this.threeInARow1.Size = new System.Drawing.Size(600, 600);
            this.threeInARow1.SizeComponent = 600;
            this.threeInARow1.TabIndex = 6;
            this.threeInARow1.Text = "threeInARow1";
            this.threeInARow1.TimerCount = 70;
            this.threeInARow1.EventScore += new System.EventHandler(this.scoreLabel_eventScore);
            this.threeInARow1.EventTimer += new System.EventHandler(this.timerLabel_eventTimer);
            this.threeInARow1.SizeChanged += new System.EventHandler(this.threeInARow1_SizeChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(874, 630);
            this.Controls.Add(this.threeInARow1);
            this.Controls.Add(this.timerLabel);
            this.Controls.Add(this.stopGameButton);
            this.Controls.Add(this.startGameButton);
            this.Controls.Add(this.scoreLabel);
            this.MinimumSize = new System.Drawing.Size(650, 490);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label scoreLabel;
        private System.Windows.Forms.Button startGameButton;
        private System.Windows.Forms.Button stopGameButton;
        private System.Windows.Forms.Label timerLabel;
        private threeInARow.ThreeInARow threeInARow1;
    }
}

