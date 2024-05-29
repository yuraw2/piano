using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp1.Properties;

namespace PianoTiles
{
    public partial class Form1 : Form
    {
        public int[,] map = new int[8, 4];          //двумерный массив выступающий в виде карты
        public int cellWidth = 50;                 //Переменные отвечающии за ширину и высоту клетки
        public int cellHeight = 80;

        public Form1()
        {
            InitializeComponent();

            this.Text = "Piano";                   //имя формы
            this.Width = cellWidth * 4 + 15;           //ширина и длина формы
            this.Height = cellHeight * 8 + 40;
            this.Paint += new PaintEventHandler(Repaint);
            this.KeyUp += new KeyEventHandler(OnKeyboardPressed);           //привязка обработчика на событие кейап
            Init();                   //старт проекта
        }

        private void OnKeyboardPressed(object sender, KeyEventArgs e)               //функция обрабатывания нажатия наших клавиш, добавления управления в нашу игру
        {
            switch (e.KeyCode.ToString())                  //через свитч проверяем какую клавишу нажали
            {
                case "D1":                              //1 на клаве
                    CheckForPressedButton(0);
                    break;
                case "D2":
                    CheckForPressedButton(1);
                    break;
                case "D3":
                    CheckForPressedButton(2);
                    break;
                case "D4":
                    CheckForPressedButton(3);
                    break;
            }
        }

        public void CheckForPressedButton(int i)            //вспомогательная функция, в которую будем передовать i, то есть нажату кнопку
        {
            if (map[7, i] != 0)
            {
                MoveMap();
                PlaySound(i);
            }
            else
            {
                MessageBox.Show("ТЫ ПРОДУЛ!");
                Init();
            }
        }

        public void PlaySound(int sound)
        {
            System.IO.Stream str = null;
            switch (sound)
            {
                case 0:                  //если пришло 0, тоесть число 1 по клавише, то вызываем звук
                    str = Resources.g6;
                    break;
                case 1:
                    str = Resources.f6;
                    break;
                case 2:
                    str = Resources.d6;
                    break;
                case 3:
                    str = Resources.e6;
                    break;
            }
            System.Media.SoundPlayer snd = new System.Media.SoundPlayer(str);         //проигрывание выбранного звука
            snd.Play();
        }

        public void MoveMap()
        {
            for (int i = 7; i > 0; i--)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = map[i - 1, j];        //каждый элемент смещаем вниз
                }
            }
            AddNewLine(); 
            Invalidate();           //перерисовка
        }

        public void AddNewLine()          //добавления новой линии
        {
            Random r = new Random();
            int j = r.Next(0, 4);
            for (int k = 0; k < 4; k++)
                map[0, k] = 0;         //вверхнюю строку полностью зануляем, потому что при смещении она скопировалась, занулировали чтобы она была пустая, а затем в эту 0 строку уже с j-ой рандомной переменной закидываем 1
            map[0, j] = 1;
        }

        public void Init()
        {
            ClearMap();
            GenerateMap();
            Invalidate();          //фунция инвалидейт чтобы выхвалось перересовка нашей формы
        }

        public void ClearMap() 
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    map[i, j] = 0;
                }
            }
        }

        public void GenerateMap()            //создание начальной карты
        {
            Random r = new Random();
            for (int i = 0; i < 8; i++)
            {
                int j = r.Next(0, 4);          //в этой переменной записывается рандомное число от 0 до 3
                map[i, j] = 1;          // 1 значит клетка будет черная
            }
        }

        public void DrawMap(Graphics g)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (map[i, j] == 0)        // если элемен карты равен 0
                    {
                        g.FillRectangle(new SolidBrush(Color.White), cellWidth * j, cellHeight * i, cellWidth, cellHeight);     //то в этом месте с этими определенными координатами i j будем заполнять прямоугольник  белого цвета, далее координаты
                    }
                    if (map[i, j] == 1)            // а если 1, то заполняем прямоугольник черным цветом
                    {
                        g.FillRectangle(new SolidBrush(Color.Black), cellWidth * j, cellHeight * i, cellWidth, cellHeight);
                    }
                }
            }
            for (int i = 0; i < 8; i++)                   // два цикла с помощью которых отрисуем вертикальные и горизонтальные линии для контуров карты, сперва 8 горизонтальных линий черного цвета с опр. координатами, по х от 0 до 4*ширину клетки, по у от i*ширину клетки до i высоту
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Black)), 0, i * cellHeight, 4 * cellWidth, i * cellHeight);
            }
            for (int i = 0; i < 4; i++)                 // 4 вертткалььные линии черного цвета, 
            {
                g.DrawLine(new Pen(new SolidBrush(Color.Black)), i * cellWidth, 0, i * cellWidth, 8 * cellHeight);
            }
        }

        private void Repaint(object sender, PaintEventArgs e)                   //функция репеинт которую мы будем привязывать на событие пэинт нашей формы
        {
            Graphics g = e.Graphics;           //графика из аргументиов
            DrawMap(g);                       //вызываем функцию драймэп с переданным аргументов графики
        }
    }
}
