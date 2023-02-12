using System;
using System.Drawing;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();
        private void startGameButton_Click(object sender, EventArgs e)
        {
            if (threeInARow1.active == false)
            {
                threeInARow1.StartGame();
            }
        }

        private void stopGameButton_Click(object sender, EventArgs e)
        {
            if (threeInARow1.active == true)
            {
                threeInARow1.Finish();
            }
        }

        private void timerLabel_eventTimer(object sender, EventArgs e)
        {
            int timer = threeInARow1.TimerCount;
            timerLabel.Text = "Таймер: " + timer;
        }

        private void scoreLabel_eventScore(object sender, EventArgs e)
        {
            int score = threeInARow1.score;
            bool access = scoreLabel.InvokeRequired;
            if(access) scoreLabel.Invoke(new Action(() => { scoreLabel.Text = "Текущий счет: " + score; }));
            else scoreLabel.Text = "Текущий счет: " + score;
        }

        private void threeInARow1_SizeChanged(object sender, EventArgs e)
        {
            if(threeInARow1.Size.Width < 300) threeInARow1.Size = new Size(300, 300);
            if (threeInARow1.Size.Width > 900) threeInARow1.Size = new Size(900, 900);
        }
    }
}

