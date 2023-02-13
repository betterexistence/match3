using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using Timer = System.Timers.Timer;

namespace threeInARow
{
    public class ThreeInARow : Control
    {
        //публичные поля
        public int score;
        public bool active;

        //защищенные поля
        private int _sizeСomponent;
        private int _timerCount;
        private static int _staticTimerCount;
        private const int GAME_FIELD_SIZE = 8;
        private bool _freeze = false;
        private Color _colorBackground;
        private Point _selectedElement = new Point(-1, -1);
        private int[,] _field = new int[GAME_FIELD_SIZE, GAME_FIELD_SIZE];
        protected Timer _timer;

        //события
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

        //публичные свойства
        public int SizeComponent
        {
            get { return _sizeСomponent; }
            set
            {

                if (value <= 300) value = 300;
                if (value >= 600) value = 600;
                if (value != _sizeСomponent)
                {
                    _sizeСomponent = value;
                    Size = new Size(_sizeСomponent, _sizeСomponent);
                }
                Invalidate();
            }
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
                Invalidate();
            }
        }
        
        //enum класс цвета элементов
        private enum BallColors : int
        {
            PURPLE = 0,
            RED,
            ORANGE,
            GREEN,
            YELLOW
        }

        //обновление очков на интерфейсе
        protected void OnScoreUpdated() => _eventScore?.Invoke(this, new EventArgs());
        //обновление счета таймера на интерфейса
        private void OnTimerElapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Invoke(new Action(() =>
            {

                if (_timerCount > 0)
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
        //конструктор класса
        public ThreeInARow()
        {
            OnScoreUpdated();
            Click += OnControlClicked;
        }

        //обновления игрового поля, проверка можно ли продолжить игру на текущем поле
        private void UpdateMatrix()
        {
            FindMatches();
            _freeze = false;
            //если при генерации ноого поля, снова нет ходов, то алгоритм обрабатывается еще раз, пока сгенерируется поле хотя бы с одним ходом
            while (!ValidateTurnOnField())
            {
                MessageBox.Show("Нет ходов", "Генерация нового поля", MessageBoxButtons.OK);
                FillMatrix();
            }
        }
        //завершение игры
        public void Finish()
        {
            _selectedElement = new Point(-1, -1);
            _timer.Elapsed -= OnTimerElapsed;
            active = false;
            MessageBox.Show("Игра завершилась. Твой счет: " + score, "Конец игры.", MessageBoxButtons.OK);
            Invalidate();
        }
        //метод старта игровой сессии
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
        //проверка выбранных элементов
        private void SelectElement()
        {
            int _elementSize = _sizeСomponent / GAME_FIELD_SIZE;
            Point mousePos = PointToClient(MousePosition);
            Point curElement = new Point(mousePos.X / (_elementSize), mousePos.Y / (_elementSize));

            //если элемент еще не выбран
            if (_selectedElement == new Point(-1, -1))
            {
                _selectedElement = curElement;
                Invalidate();
                return;
            }
            //если выбранный элемент не текущий
            if (_selectedElement != curElement)
            {
                //проверка, что элемент находится рядом
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
        //падение элементов и генерация новых
        private void FallElements()
        {
            Random random = new Random();
            Thread.Sleep(100); //создание визуальной анимации падения
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
                        //если все элементы упали, то генерация нового элемента
                        _field[row, col] = random.Next(0, 5);
                    }
                }
                Thread.Sleep(100);
                Invalidate();
            }
            UpdateMatrix();
        }
        //проверка ходов на игровом поле
        private bool ValidateTurnOnField()
        {
            bool isMatch = false;
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    //проверки для текущего элемента
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
            //все проверои по горизонтали
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
            //все проверки по вертикали
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
        //нахождение и удаление одинаковых рядов
        private bool FindMatches()
        {
            bool isMatch = false;
            int[,] tempField = _field.Clone() as int[,];
            //поиск вертикальных рядов
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
            //поиск горизонтальных рядов
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
            //если нет рядов, то выход из метода
            if (!isMatch) return false;
            //подсчет очков
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
        //проверка на обмен позиций элементов
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
                if (!FindMatches()) //если при обмене элементов не образовалось рядов, то возвращает позиции
                {
                    SwapElements(firstElem.Y, firstElem.X, secondElem.Y, secondElem.X);
                    _freeze = false;
                }
            };
        }
        //обмен позиций элементов
        private void SwapElements(int x1, int y1, int x2, int y2)
        {
            int temp = _field[x1, y1];
            _field[x1, y1] = _field[x2, y2];
            _field[x2, y2] = temp;
            Invalidate();
        }
        //заполнение игрового поля
        private void FillMatrix()
        {
            Random random = new Random();
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    //заполнение поля без рядов из трех элементов
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
        //проверка осуществление клика по игровому полю
        private void OnControlClicked(object sender, EventArgs e)
        {
            if (active)
                if (!_freeze)
                    SelectElement();
        }
        //отрисовка игрового поля
        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.FillRectangle(new SolidBrush(_colorBackground), 0, 0, _sizeСomponent, _sizeСomponent);
            if (!active) return;
            int _elementSize =  _sizeСomponent / GAME_FIELD_SIZE;
            int indent = _elementSize / GAME_FIELD_SIZE;
            for (int row = 0; row < GAME_FIELD_SIZE; row++)
            {
                for (int col = 0; col < GAME_FIELD_SIZE; col++)
                {
                    BallColors ballColor = (BallColors)_field[row, col];
                    Brush brush = Brushes.Transparent;

                    if (ballColor == BallColors.PURPLE) brush = Brushes.DarkViolet;
                    else if (ballColor == BallColors.RED) brush = Brushes.Crimson;
                    else if (ballColor == BallColors.ORANGE) brush = Brushes.Coral;
                    else if (ballColor == BallColors.GREEN) brush = Brushes.YellowGreen;    
                    else if (ballColor == BallColors.YELLOW) brush = Brushes.LemonChiffon;

                    if (_selectedElement.X == col && _selectedElement.Y == row)
                        e.Graphics.FillRectangle(Brushes.Black, col * _elementSize + 2, row * _elementSize + 2, _elementSize, _elementSize);

                    e.Graphics.FillRectangle(brush, col * _elementSize + indent + 2, row * _elementSize + indent + 2, _elementSize - indent * 2 , _elementSize - indent *2);
                }
            }
        }
        //размеры компонента
        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            if (width > Math.Min(height, width)) width = height;
            else height = width;
            base.SetBoundsCore(x, y, width, height, specified);
            _sizeСomponent = width;
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