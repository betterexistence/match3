using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;
using System.Windows.Forms;

namespace threeInARow
{
    public class threeinarow : Control
    {
        protected int _timerCount; //количество секунд
        protected Timer _timer; 
        protected bool _active;
        protected int _elementSize;
        protected const int fieldSize = 8;
        protected int _score;

        public delegate void ElementRemoveHandler(int x, int y);
        public event ElementRemoveHandler ElementRemoved;

        public delegate void MatchesRemoveHandler();
        public event MatchesRemoveHandler MatchesRemoved;

        public delegate void ElementsFallHandler(List<Element> elements);
        public event ElementsFallHandler ElementsFalled;

        protected int _x;
        protected int _y;
        protected int[,] field = new int[fieldSize, fieldSize];
        private Point _firstElement = new Point(-1, -1);
        public Point item;

        static threeinarow checkedElement = null;

        threeinarow[,] elements;

        public threeinarow()
        {
            _score = 0;
            _timerCount = 60;
            _timer = new Timer();
            _timer.Interval = 1000;
            _x = 0;
            _y = 0;
            _active = true;
            ElementRemoved += Game_ElementRemoved;
            MatchesRemoved += Game_MatchesRemoved;
            ElementsFalled += Game_ElementsFalled;
            FillMatrix();
        }

        private void Game_ElementRemoved(int x, int y)
        {
            field[y, x] = -1;
        }

        private void Game_MatchesRemoved()
        {
            //scoreLabel.Text = "Score: " + m_game.GetScore().ToString();
            UpdateElements();
            Fall();
            //Active = true;
        }

        private void Game_ElementsFalled(List<Element> indices)
        {
            List<int> elements = new List<int>();
            foreach (Element element in indices)
                elements.Add(field[element.Y, element.X]);
            if (Fall())
            {
                UpdateElements();
            }
            else if (RemoveMatches() == false)
                Active = true;
            UpdateElements();
        }

        public sbyte GetValue(int x, int y)
        {
            return (sbyte)field[y, x];
        }

        public void UpdateElements()
        {
            int _elementSize = Width / 8;
            Point _element = new Point(MousePosition.X / (_elementSize), MousePosition.Y / (_elementSize));
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    //field[y, x] = field[_element.X, _element.Y];
                    int value = GetValue(x, y);
                    if (value >= 0)
                        field[y, x] = GetValue(x, y);
                    else
                        field[y, x] = -1;
                }
            Invalidate();
        }

        public bool Active
        {
            get { return _active; }
            set
            {
                //игра может завершиться, только когда активна
                if (_active == false && value == true && _timerCount <= 0)
                    Finish();
                else
                    _active = value;
            }
        }

        public bool RemoveMatches()
        {
            List<Line> lines = new List<Line>();
            //поиск горизонтальных линий
            int[,] tmpMatrix = (int[,])field.Clone();
            for (int y = 0; y < fieldSize; y++)
                for (int x = 0; x < fieldSize; x++)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = x + 1; i < fieldSize; i++)
                        if (tmpMatrix[y, i] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = x; i < x + count; i++)
                            tmpMatrix[y, i] = -1;
                        lines.Add(new Line(new Element(x, y), new Element(x + count - 1, y)));
                    }
                }

            //поиск вертикальных линий
            tmpMatrix = (int[,])field.Clone();
            for (int y = 0; y < fieldSize; y++)
                for (int x = 0; x < fieldSize; x++)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = y + 1; i < fieldSize; i++)
                        if (tmpMatrix[i, x] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = y; i < y + count; i++)
                            tmpMatrix[i, x] = -1;
                        lines.Add(new Line(new Element(x, y), new Element(x, y + count - 1)));
                    }
                }

            if (lines.Count == 0)
                return false;

            int baseValue = 10;
            foreach (Line line in lines)
            {
                int count = 0;
                //горизонтальная линия
                if (line.Start.Y == line.Finish.Y)
                    for (int i = line.Start.X; i <= line.Finish.X; i++)
                    {
                        field[line.Start.Y, i] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(i, line.Start.Y);
                        count++;
                    }
                //вертикальная линия
                else
                    for (int i = line.Start.Y; i <= line.Finish.Y; i++)
                    {
                        field[i, line.Start.X] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(line.Start.X, i);
                        count++;
                    }
                int value = (count - 2) * baseValue;
                _score += count * value;
            }
            if (MatchesRemoved != null)
                MatchesRemoved();
            return true;
        }

        public bool Fall()
        {
            List<Element> elements = new List<Element>();
            //сдвигаем каждый элемент вниз, если есть место
            for (int y = fieldSize - 2; y >= 0; --y)
                for (int x = fieldSize - 1; x >= 0; --x)
                {
                    if (field[y + 1, x] == -1)
                    {
                        field[y + 1, x] = field[y, x];
                        field[y, x] = -1;
                        elements.Add(new Element(x, y + 1));
                    }
                }
            //добавляем новые элементы сверху
            Random random = new Random();
            for (int x = 0; x < fieldSize; ++x)
                if (field[0, x] == -1)
                {
                    field[0, x] = random.Next(0, 5);
                    elements.Add(new Element(x, 0));
                }
            if (ElementsFalled != null && elements.Count != 0)
                ElementsFalled(elements);
            if (elements.Count == 0)
                return false;
            else
                return true;
        }

        //обмен
        public void swap(Element a, Element b)
        {
            int temp = field[a.X, a.Y];
            field[a.X, a.Y] = field[b.X, b.Y];
            field[b.X, b.Y] = temp;
        }

        //заполнение матрицы без повторений
        public void FillMatrix()
        {
            Random random = new Random();
            for (int y = 0; y < fieldSize; y++)
            {
                for(int x = 0; x < fieldSize; x++)
                {
                    int value;
                    bool repeat = false;
                    do
                    {
                        value = random.Next(0, 5);
                        repeat = false;
                        if (x >= 2 && (field[y, x - 2] == field[y, x - 1] && field[y, x - 2] == value))
                            repeat = true;
                        else if (y >= 2 && (field[y - 2, x] == field[y - 1, x] && field[y - 2, x] == value))
                            repeat = true;
                    }
                    while (repeat != false);
                    field[y, x] = value;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Brush backgroundColor = new SolidBrush(Color.Transparent);
            e.Graphics.FillRectangle(backgroundColor, 0, 0, Width, Height);
            int _elementSize = Size.Width / fieldSize;
            int rectXY = _elementSize;
            x = 5;
            for (int i = 0; i < fieldSize; i++)
            {
                y = 5;
                for (int j = 0; j < fieldSize; j++)
                {
                    if (field[i, j] == 0)
                        e.Graphics.FillEllipse(Brushes.Purple, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    else if (field[i, j] == 1)
                            e.Graphics.FillEllipse(Brushes.Red, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    else if (field[i, j] == 2)
                            e.Graphics.FillEllipse(Brushes.Orange, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    else if (field[i, j] == 3)
                            e.Graphics.FillEllipse(Brushes.Green, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    else if (field[i, j] == 4)
                            e.Graphics.FillEllipse(Brushes.Yellow, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    if(field[i,j] == 5)
                    {
                        e.Graphics.FillEllipse(Brushes.Transparent, _x + 5, _y + 5, _elementSize - 10, _elementSize - 10);
                    }
                    _y += rectXY;
                }
                _x += rectXY;
            }
        }

        public int x
        {
            get
            {
                return _x;
            }
            set
            {
                if (value < 0) { value = 0; }
                else
                {
                    if (value > Size.Width - Size.Width / 8) { value = Size.Width - Size.Width / 8; }
                }
                if (_x != value)
                {
                    _x = value;
                    Invalidate();
                }
            }
        }
        public int y
        {
            get
            {
                return _y;
            }
            set
            {
                if (value < 0) { value = 0; }
                else
                {
                    if (value > Size.Width - Size.Width / 8) { value = Size.Width - Size.Width / 8; }
                }
                if (_y != value)
                {
                    _y = value;
                    Invalidate();
                }
            }
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
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (width > Math.Min(width, height))
            {
                width = height;
            }
            else
            {
                height = width;
            }
            base.SetBoundsCore(x, y, width, height, specified);
            this.x = _x;
            this.y = _y;
            Invalidate();
        }

        public void Movefield(Point MousePosition)
        {
            int _elementSize = Width / 8;
            MousePosition = PointToClient(MousePosition);
            Point _element = new Point(MousePosition.X / (_elementSize), MousePosition.Y / (_elementSize));

            if (_firstElement.X == -1 || _firstElement.Y == -1) //первая фигура выбрана
            {
                _firstElement = _element;
                return;
            }

            if (field[_element.X, _element.Y] == field[_firstElement.X, _firstElement.Y]) //проверка на второй клик по той же фигуре
                return;

            if (_firstElement != _element)
            {
                bool near = false;
                if ((_element.X == _firstElement.X && _element.Y == _firstElement.Y - 1) ||
                    (_element.X == _firstElement.X && _element.Y == _firstElement.Y + 1) ||
                    (_element.X == _firstElement.X - 1 && _element.Y == _firstElement.Y) ||
                    (_element.X == _firstElement.X + 1 && _element.Y == _firstElement.Y))
                    near = true;

                if (!near)
                {
                    _firstElement = _element;
                }
                else
                {
                    _active = false;
                    moveElements(_firstElement.X, _firstElement.Y, _element.X, _element.Y);
                    //_firstElement = new Point(-1, -1);
                }
            }

            _firstElement = new Point(-1, -1);
            Invalidate();
        }

        private void moveElements(int el1X, int el1Y, int el2X, int el2Y)
        {
            Element elementA = new Element(el1X, el1Y);
            Element elementB = new Element(el2X, el2Y);
            swap(elementA, elementB);

            bool result = RemoveMatches();
            if(result == false)
            {
                swap(elementB, elementA);
            }
        }

        public void removeElement(int row, int col)
        {
            field[row, col] = 5;
        }

        public void Finish()
        {
            _active = false;
            //MessageBox.Show("Игра завершилась. Твой счет: " + score, "Game over", MessageBoxButtons.OK);
        }

    }
}
