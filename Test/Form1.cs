using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using threeInARow;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void startGameButton_Click(object sender, EventArgs e)
        {
            if (threeinarow1.active == false)
            {
                threeinarow1.StartGame();
            }
        }

        private void stopGameButton_Click(object sender, EventArgs e)
        {
            if (threeinarow1.active == true)
            {
                threeinarow1.Finish();
            }
        }

        private void timerLabel_eventTimer(object sender, EventArgs e)
        {
            int timer = threeinarow1._timerCount;
            timerLabel.Text = "Таймер: " + timer;
        }

        private void scoreLabel_eventScore(object sender, EventArgs e)
        {
            int score = threeinarow1.score;
            bool access = scoreLabel.InvokeRequired;
            if(access) scoreLabel.Invoke(new Action(() => { scoreLabel.Text = "Текущий счет: " + score; }));
            else scoreLabel.Text = "Текущий счет: " + score;
        }
    }
}

