﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1() => InitializeComponent();

        private void startGameButton_Click(object sender, EventArgs e)
        {

        }

        private void stopGameButton_Click(object sender, EventArgs e)
        {

        }

        private void scoreLabel_eventScore(object sender, EventArgs e)
        {
            int score = threeinarow1.Score;
            scoreLabel.Text = "Очков: " + score;
        }
    }
}

