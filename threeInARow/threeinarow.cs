using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Timers.Timer;

namespace threeInARow
{
    public class ThreeInARow : Control
    {
        public int score;
        public int _timerCount;
        public bool active;
         
        private static int _staticTimerCount;
        private const int GAME_FIELD_SIZE = 8;
        private bool _freeze = false;
        private Color _colorBackground;
        private Point _selectedElement = new Point(-1, -1);
        private int[,] _field = new int[GAME_FIELD_SIZE, GAME_FIELD_SIZE];
        
        protected Timer _timer;
        protected event EventHandler _eventScore;
        public event EventHandler EventScore
        {
            add { _eventScore += value; }
            remove { _eventScore -= value; }
        }
        protected event EventHandler _eventTimer;
        public event EventHandler EventTimer
        {
            add { _eventTimer += value; }
            remove { _eventTimer -= value; }
        }

        public int TimerCount
        {
            get => _timerCount;
            set
            {
                _timerCount = value;
                if(_timerCount < 60) _timerCount = 60;
                if (_timerCount > 180) _timerCount = 180;
                _staticTimerCount = _timerCount;
            }
        }
        public Color ColorBackground
        {
            get => _colorBackground;
            set
            {
                _colorBackground = value;
            }
        }
        private enum BallColors : int
        {
            PURPLE = 0,
            RED,
            ORANGE,
            GREEN,
            YELLOW
        }

        protected void OnScoreUpdated() => _eventScore?.Invoke(this, new EventArgs());
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {

                if (_timerCount > 0 && !_freeze)
                {
                    _timerCount--;
                    _eventTimer?.Invoke(this, new EventArgs());
                }
                else if (active && !_freeze)
                {
                    Finish();
                }
            }));
        }

        public ThreeInARow()
        {
            OnScoreUpdated();
            Click += OnControlClicked;
        }

        private void UpdateMatrix()
        {
            FindMatches();
            _freeze = false;
            while (!ValidateTurnOnField())
            {
                MessageBox.Show("Нет ходов", "Генерация нового поля", MessageBoxButtons.OK);
                FillMatrix();
            }   
        }

        public void Finish()
        {
            _selectedElement = new Point(-1, -1);
            _timer.Elapsed -= OnTimerElapsed;
            active = false;
            MessageBox.Show("Игра завершилась. Твой счет: " + score, "Конец игры.", MessageBoxButtons.OK);
        }

        public void StartGame()
        {
            score = 0;
            _timerCount = _staticTimerCount;
            _timer = new Timer(1000);
            _timer.Start();
            _timer.Elapsed += OnTimerElapsed;
            FillMatrix();
            ValidateTurnOnField();
            active = true;
            OnScoreUpdated();
        }

        public void SelectElement()
        {
            int _elementSize = Width / GAME_FIELD_SIZE;
            Point mousePos = PointToClient(MousePosition);
            Point curElement = new Point(mousePos.X / (_elementSize), mousePos.Y / (_elementSize));

            if (_selectedElement == new Point(-1, -1))
            {
                _selectedElement = curElement;
                Invalidate();
                return;
            }

            if (_selectedElement != curElement)
            {
                if ((curElement.X == _selectedElement.X && curElement.Y == _selectedElement.Y - 1) ||
                    (curElement.X == _selectedElement.X && curElement.Y == _selectedElement.Y + 1) ||
                    (curElement.X == _selectedElement.X - 1 && curElement.Y == _selectedElement.Y) ||
                    (curElement.X == _selectedElement.X + 1 && curElement.Y == _selectedElement.Y))
                {
                    MoveElements(_selectedElement, curElement);
                    _selectedElement = new Point(-1, -1);
                }
                else _selectedElement = curElement;
            }
            else _selectedElement = new Point(-1, -1);
            Invalidate();
        }

        private void FallElements()
        {
            Random random = new Random();
            Thread.Sleep(100);
            for (int row = GAME_FIELD_SIZE - 1; row >= 0; --row)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    if (_field[row, col] != -1) continue;
                    Thread.Sleep(50);
                    int i = 0;
                    do
                    {   
                        i++;
                    } while (row - i >= 0 && _field[row - i, col] == -1);

                    if (row - i >= 0 && _field[row - i, col] != -1)
                    {
                        _field[row, col] = _field[row - i, col];
                        _field[row - i, col] = -1;
                    }
                    else
                    {
                        _field[row, col] = random.Next(0, 5);
                    }
                }
                Thread.Sleep(100);
                Invalidate();
            }
            UpdateMatrix();
        }

        private bool ValidateTurnOnField()
        {
            bool isMatch = false;
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    int cur = _field[row, col];
                    if (row - 1 >= 0)
                        if (CheckRow(row, row - 1, col, cur)) isMatch = true;

                    if (row + 1 < GAME_FIELD_SIZE)
                        if (CheckRow(row, row + 1, col, cur)) isMatch = true;

                    if (col - 1 >= 0)
                        if (CheckCol(col, row, col - 1, cur)) isMatch = true;

                    if (col + 1 < GAME_FIELD_SIZE)
                        if (CheckCol(col, row, col + 1, cur)) isMatch = true;
                }
            }
            bool CheckRow(int old, int row, int col, int cur)
            {
                bool _isMatch = false;
                if (col - 2 >= 0 && _field[row, col - 2] == cur && _field[row, col - 1] == cur) _isMatch = true;
                else if (col - 1 >= 0 && col + 1 < GAME_FIELD_SIZE && _field[row, col - 1] == cur && _field[row, col + 1] == cur) _isMatch = true;
                else if (col + 2 < GAME_FIELD_SIZE && _field[row, col + 1] == cur && _field[row, col + 2] == cur) _isMatch = true;
                else if (old < row && row + 2 < GAME_FIELD_SIZE && _field[row + 1, col] == cur && _field[row + 2, col] == cur) _isMatch = true;
                else if (old > row && row - 2 >= 0 && _field[row - 2, col] == cur && _field[row - 1, col] == cur) _isMatch = true;
                return _isMatch;
            }
            bool CheckCol(int old, int row, int col, int cur)
            {
                bool _isMatch = false;
                if (row - 2 >= 0 && _field[row - 2, col] == cur && _field[row - 1, col] == cur) _isMatch = true;
                else if (row - 1 >= 0 && row + 1 < GAME_FIELD_SIZE && _field[row - 1, col] == cur && _field[row + 1, col] == cur) _isMatch = true;
                else if (row + 2 < GAME_FIELD_SIZE && _field[row + 1, col] == cur && _field[row + 2, col] == cur) _isMatch = true;
                else if (old < col && col + 2 < GAME_FIELD_SIZE && _field[row, col + 1] == cur && _field[row, col + 2] == cur) _isMatch = true;
                else if (old > col && col - 2 >= 0 && _field[row, col - 2] == cur && _field[row, col - 1] == cur) _isMatch = true;
                return _isMatch;
            }
            return isMatch;
        }

        private bool FindMatches()
        {
            bool isMatch = false;
            int[,] tempField = _field.Clone() as int[,];
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                int countCol = 1;
                for (int col = 1; col < GAME_FIELD_SIZE; col++)
                {
                    if (_field[row, col - 1] == _field[row, col]) countCol++;
                    else countCol = 1;

                    if (countCol == 3)
                    {
                        tempField[row, col - 2] = -1;
                        tempField[row, col - 1] = -1;
                        tempField[row, col] = -1;
                        isMatch = true;
                    }
                    else if (countCol > 3) tempField[row, col] = -1;
                }
            }

            for (int col = 0; col < GAME_FIELD_SIZE; col++)
            {
                int countRow = 1;
                for (int row = 1; row < GAME_FIELD_SIZE; row++)
                {
                    if (_field[row - 1, col] == _field[row, col]) countRow++;
                    else countRow = 1;

                    if (countRow == 3)
                    {
                        tempField[row - 2, col] = -1;
                        tempField[row - 1, col] = -1;
                        tempField[row, col] = -1;
                        isMatch = true;
                    }
                    else if (countRow > 3) tempField[row, col] = -1;
                }
            }

            if (!isMatch) return false;

            int value = 10;
            int count = 0;
            foreach (int i in tempField)
                if (i == -1)
                    count++;

            score += count * value;
            _field = tempField.Clone() as int[,];
            FallElements();
            OnScoreUpdated();
            return true;
        }

        private void MoveElements(Point firstElem, Point secondElem)
        {
            _freeze = true;
            SwapElements(firstElem.Y, firstElem.X, secondElem.Y, secondElem.X);
            Timer timer = new Timer();
            timer.Interval = 500;
            timer.AutoReset = false;
            timer.Start();  
            timer.Elapsed += (o, ev) =>
            {
                if (!FindMatches())
                {
                    SwapElements(firstElem.Y, firstElem.X, secondElem.Y, secondElem.X);
                    _freeze = false;
                }
            };
        }
        private void SwapElements(int x1, int y1, int x2, int y2)
        {
            int temp = _field[x1, y1];
            _field[x1, y1] = _field[x2, y2];
            _field[x2, y2] = temp;
            Invalidate();
        }
        private void FillMatrix()
        {
            Random random = new Random();
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    int value;
                    bool repeat = false;
                    do
                    {
                        value = random.Next(0, 5);
                        repeat = false;
                        if (col >= 2 && _field[row, col - 2] == _field[row, col - 1] && _field[row, col - 2] == value) repeat = true;
                        else if (row >= 2 && _field[row - 2, col] == _field[row - 1, col] && _field[row - 2, col] == value) repeat = true;
                    }
                    while (repeat);

                    _field[row, col] = value;
                }   
            }
            Invalidate();
        }
        private void OnControlClicked(object sender, EventArgs e)
        {
            if (active)
                if (!_freeze)
                    SelectElement();
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            if (!active) return;
            int _elementSize = Size.Width / GAME_FIELD_SIZE;
            e.Graphics.FillRectangle(new SolidBrush(_colorBackground), 0, 0, Size.Width, Size.Height);
            int indent = _elementSize / GAME_FIELD_SIZE;
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    BallColors ballColor = (BallColors)_field[row, col];
                    Brush brush = Brushes.Transparent;
                    Brush brushLight = Brushes.Transparent;
                    Brush brushDark = Brushes.Transparent;

                    if (ballColor == BallColors.PURPLE)
                    {
                        brushLight = Brushes.Violet;
                        brush = Brushes.DarkViolet;
                        brushDark = Brushes.Purple;
                    }
                    else if (ballColor == BallColors.RED) brush = Brushes.Crimson;
                    else if (ballColor == BallColors.ORANGE) brush = Brushes.Coral;
                    else if (ballColor == BallColors.GREEN) brush = Brushes.YellowGreen;
                    else if (ballColor == BallColors.YELLOW) brush = Brushes.LemonChiffon;

                    if (_selectedElement.X == col && _selectedElement.Y == row)
                        e.Graphics.FillRectangle(Brushes.Black, col * _elementSize, row * _elementSize, _elementSize, _elementSize);

                    //e.Graphics.FillRectangle(brushLight, col * _elementSize + indent, row * _elementSize + indent, _elementSize - indent * 2, _elementSize - indent * 2);
                    //e.Graphics.FillRectangle(brushDark, col * _elementSize + indent + 5, row * _elementSize + indent + 5, _elementSize - indent * 2, _elementSize - indent * 2);
                    e.Graphics.FillRectangle(brush, col * _elementSize + indent , row * _elementSize + indent, _elementSize - indent * 2 , _elementSize - indent *2);
                }
            }
        }
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (width > Math.Min(width, height)) width = height;
            else height = width;

            base.SetBoundsCore(x, y, width, height, specified);
            Invalidate();
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;
                return cp;
            }
        }
    }
}