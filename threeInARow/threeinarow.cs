using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.LinkLabel;

namespace threeInARow
{
    public class ThreeInARow : Control
    {
        private const int GAME_FIELD_SIZE = 8;
        private int _score;

        private enum BallColors : int
        {
            PURPLE = 0,
            RED,
            ORANGE,
            GREEN,
            YELLOW
        }

        // TODO: Check events
        public delegate void ElementRemoveHandler();
        public event ElementRemoveHandler ElementRemoved;

        public delegate void ElementsFallHandler();
        public event ElementsFallHandler ElementsFalled;

        public int Score
        {
            get => _score;
            set
            {
                if (value < 0) value = 0;
                _score = value;
            }
        }

        private int[,] _field = new int[GAME_FIELD_SIZE, GAME_FIELD_SIZE];  // ROW | COLUMN

        private Point _selectedElement = new Point(-1, -1);

        public ThreeInARow()
        {
            Score = 0;
            Click += OnControlClicked;
            ElementsFalled += OnElementsFalled;
            ElementRemoved += OnElementRemoved;
            FillMatrix();


            //_isGamePlaying = true;
        }

        private void OnElementRemoved()
        {
            ElementRemoved?.Invoke();
        }

        private void OnElementsFalled()
        {
            FindMatches();
            Invalidate();
            // Уведомить пользователя + вызвать FillMatrix();
        }

        public void SelectElement()
        {
            int _elementSize = Width / 8;
            Point mousePos = PointToClient(MousePosition);
            Point curElement = new Point(mousePos.X / (_elementSize), mousePos.Y / (_elementSize));

            // Если элемент еще не выбран
            if (_selectedElement == new Point(-1, -1))
            {
                _selectedElement = curElement;
                return;
            }

            if (_selectedElement != curElement)
            {
                if ((curElement.X == _selectedElement.X && curElement.Y == _selectedElement.Y - 1) ||
                    (curElement.X == _selectedElement.X && curElement.Y == _selectedElement.Y + 1) ||
                    (curElement.X == _selectedElement.X - 1 && curElement.Y == _selectedElement.Y) ||
                    (curElement.X == _selectedElement.X + 1 && curElement.Y == _selectedElement.Y))
                    MoveElements(_selectedElement, curElement);
            }

            _selectedElement = new Point(-1, -1);
        }

        private void FallElements()
        {
            Random random = new Random();
            for (int col = 0; col < GAME_FIELD_SIZE; ++col)
            {
                for (int row = GAME_FIELD_SIZE - 1; row >= 0; --row)
                {
                    if (_field[row, col] != -1) continue;

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
                    else _field[row, col] = random.Next(0, 5);
                }
            }

            ElementsFalled?.Invoke();
        }

        private bool FindMatches()
        {
            bool isMatch = false;
            int[,] tempField = _field.Clone() as int[,];
            for (int row = 0; row < GAME_FIELD_SIZE; ++row)
            {
                int count = 1;
                for (int col = 1; col < GAME_FIELD_SIZE; ++col)
                {
                    if (tempField[row, col - 1] == tempField[row, col]) count++;
                    else count = 1;

                    if (count == 3)
                    {
                        tempField[row, col - 2] = -1;
                        tempField[row, col - 1] = -1;
                        tempField[row, col] = -1;
                        isMatch = true;
                    }
                    else if (count > 3) tempField[row, col] = -1;
                }
            }

            for (int col = 0; col < GAME_FIELD_SIZE; ++col)
            {
                int count = 1;
                for (int row = 1; row < GAME_FIELD_SIZE; ++row)
                {
                    if (tempField[row - 1, col] == tempField[row, col]) count++;
                    else count = 1;

                    if (count == 3)
                    {
                        tempField[row - 2, col] = -1;
                        tempField[row - 1, col] = -1;
                        tempField[row, col] = -1;
                        isMatch = true;
                    }
                    else if (count > 3) tempField[row, col] = -1;
                }
            }

            if (isMatch)
            {
                _field = tempField.Clone() as int[,];
                FallElements();
            };

            return isMatch;
        }

        private void SwapArrayElements(int x1, int y1, int x2, int y2)
        {
            int temp = _field[x1, y1];
            _field[x1, y1] = _field[x2, y2];
            _field[x2, y2] = temp;
        }

        private void MoveElements(Point firstElem, Point secondElem)
        {
            SwapArrayElements(firstElem.Y, firstElem.X, secondElem.Y, secondElem.X);
            if (!FindMatches()) SwapArrayElements(firstElem.Y, firstElem.X, secondElem.Y, secondElem.X);
        }

        /// <summary>
        /// Заполнение матрица
        /// TODO: сделать так, чтобы были ходы!
        /// </summary>
        public void FillMatrix()
        {
            Random random = new Random();
            for (int row = 0; row < GAME_FIELD_SIZE; ++row)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; ++col)
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

        private void OnControlClicked(object sender, EventArgs e) => SelectElement();

        protected override void OnPaint(PaintEventArgs e)
        {
            int _elementSize = Size.Width / GAME_FIELD_SIZE;

            for (int row = 0; row < GAME_FIELD_SIZE; ++row)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; ++col)
                {
                    BallColors ballColor = (BallColors)_field[row, col];
                    Brush brush = Brushes.Transparent;

                    if (ballColor == BallColors.PURPLE) brush = Brushes.Purple;
                    else if (ballColor == BallColors.RED) brush = Brushes.Red;
                    else if (ballColor == BallColors.ORANGE) brush = Brushes.Orange;
                    else if (ballColor == BallColors.GREEN) brush = Brushes.Green;
                    else if (ballColor == BallColors.YELLOW) brush = Brushes.Yellow;

                    e.Graphics.FillEllipse(brush, col * _elementSize, row * _elementSize, _elementSize - 5, _elementSize - 5);
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
