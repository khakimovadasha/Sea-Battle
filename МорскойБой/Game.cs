﻿using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace МорскойБой
{
    public partial class Game : Form
    {
        Battle objBattle = new Battle();
        public Game()
        {
            InitializeComponent();
            UpdateShipPlacementOrder();
        }
        public static bool najat = false;
		public static bool gotov = false;
		private void panel1_Paint(object sender, PaintEventArgs e)
        {
            int w = panel1.Width/objBattle.ReturnMasSize();
            int h = panel1.Height/objBattle.ReturnMasSize();
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(w, 1), Color.White);
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(1, h), Color.White);
            for (int i = 0; i < objBattle.ReturnMasSize(); i++)
                for (int j = 0; j < objBattle.ReturnMasSize(); j++)
                {
                    if (objBattle[i,j] == 0)
                        e.Graphics.FillRectangle(Brushes.White, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 1 || objBattle[i,j] == 2)
                        e.Graphics.FillRectangle(Brushes.Green, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 3)
                        e.Graphics.FillRectangle(Brushes.Red, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle[i, j] == 4)
                        e.Graphics.FillRectangle(Brushes.Black, j * w + 1, i * h + 1, w - 1, h - 1);
                }
        }
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            int w = panel1.Width / objBattle.ReturnMasSize();
            int h = panel1.Height / objBattle.ReturnMasSize();
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(w, 1), Color.White);
            ControlPaint.DrawGrid(e.Graphics, new Rectangle(Point.Empty, panel1.Size), new Size(1, h), Color.White);
            for (int i = 0; i < objBattle.ReturnMasSize(); i++)
                for (int j = 0; j < objBattle.ReturnMasSize(); j++)
                {
                    if (objBattle.GetEnemyValue(i, j) < 3)
                        e.Graphics.FillRectangle(Brushes.White, j * w + 1, i * h + 1, w - 1, h - 1);
                    //if (objBattle.GetEnemyValue(i, j) == 2)
                        //e.Graphics.FillRectangle(Brushes.Green, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle.GetEnemyValue(i, j) == 3)
                        e.Graphics.FillRectangle(Brushes.Red, j * w + 1, i * h + 1, w - 1, h - 1);
                    if (objBattle.GetEnemyValue(i, j) == 4)
                        e.Graphics.FillRectangle(Brushes.Black, j * w + 1, i * h + 1, w - 1, h - 1);
                }
        }

        private void Game_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!najat)
            {
                if (!(radioButton1.Enabled == false && radioButton2.Enabled == false && radioButton3.Enabled == false
                    && radioButton4.Enabled == false))
                {
                    int w = panel1.Width / objBattle.ReturnMasSize();
                    int h = panel1.Height / objBattle.ReturnMasSize();
                    int x = e.X / w;
                    int y = e.Y / h;
                    int size;
                    if (radioButton1.Checked) size = 1;
                    else if (radioButton2.Checked) size = 2;
                    else if (radioButton3.Checked) size = 3;
                    else size = 4;
                    objBattle.WatchShip(y, x, w, h, size, panel1);
                }
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            if (!najat)
            {
                objBattle.NullMas();
                panel1.Invalidate();
            }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (!najat)
            {
                if (e.Button == MouseButtons.Right)
                {
                    objBattle.ChangeVerctical();
                    objBattle.NullMas();
                    panel1.Invalidate();
                }
                else
                {
                    int w = panel1.Width / objBattle.ReturnMasSize();
                    int h = panel1.Height / objBattle.ReturnMasSize();
                    int x = e.X / w;
                    int y = e.Y / h;
                    int size;
                    if (radioButton1.Checked) size = 1;
                    else if (radioButton2.Checked) size = 2;
                    else if (radioButton3.Checked) size = 3;
                    else size = 4;
                    objBattle.NullMas();
                    if (objBattle.CheckSq(x, y, size - 1, objBattle.GetLink()))
                    {
                        objBattle.CreateShip(x, y, 2, size - 1, objBattle.GetLink());
                        if (radioButton1.Checked) objBattle.Count1X++;
                        else if (radioButton2.Checked) objBattle.Count2X++;
                        else if (radioButton3.Checked) objBattle.Count3X++;
                        else objBattle.Count4X++;
                        UpdateShipPlacementOrder();
                    }
                    panel1.Invalidate();
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            objBattle.NullMas();
            panel1.Invalidate();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            objBattle.Reset();
            UpdateShipPlacementOrder();
            panel1.Invalidate();
            labelScore.Text = "Очки: 0";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (gotov)
            {
                label1.Text = @"Играем против компьютера";
                label1.Location = new Point(label1.Location.X - 75, label1.Location.Y);
                objBattle.Count1X = 0;
                objBattle.Count2X = 0;
                objBattle.Count3X = 0;
                objBattle.Count4X = 0;
                najat = true;
                int y, x, count = 10, size = 3;
                while (count > 0)
                {
                    Random r = new Random();
                    if (count < 10) size = 2;
                    if (count < 8) size = 1;
                    if (count < 5) size = 0;
                    y = r.Next(0, 10);
                    x = r.Next(0, 10);
                    if (r.Next(0, 2) == 1)
                        objBattle.ChangeVerctical();
                    if (objBattle.CheckSq(x, y, size, objBattle.GetEnemyLink()))
                    {
                        objBattle.CreateShip(x, y, 2, size, objBattle.GetEnemyLink());
                        count--;
                    }
                }
                panel2.Invalidate();
                buttonReady.Enabled = false;
                buttonReset.Enabled = false;
                labelScore.Text = "Очки: 0";
            }
        }

        private void panel2_MouseClick(object sender, MouseEventArgs e)
        {
            if (najat)
            {
                if (e.Button == MouseButtons.Left == true)
                {
                    int w = panel1.Width / objBattle.ReturnMasSize();
                    int h = panel1.Height / objBattle.ReturnMasSize();
                    int x = e.X / w;
                    int y = e.Y / h;
                    if (objBattle.GetEnemyValue(y, x) == 2)
                    {
                        objBattle.SetEnemyValue(y, x, 3);
                        objBattle.EnemyShip--;
                        
                        // Проверяем, потоплен ли корабль
                        bool isShipSunk = CheckIfShipSunk(x, y, objBattle.GetEnemyLink());
                        int shipSize = isShipSunk ? GetShipSize(x, y, objBattle.GetEnemyLink()) : 0;
                        
                        // Обновляем очки
                        objBattle.UpdateScore(true, shipSize);
                        
                        objBattle.Explosion(x, y, objBattle.GetEnemyLink());
                        panel2.Invalidate();
                        
                        // Обновляем отображение очков
                        labelScore.Text = "Очки: " + objBattle.PlayerScore.ToString();
                        
                        if (objBattle.EnemyShip == 0)
                            Victory("Победил человек");
                    }
                    else if (objBattle.GetEnemyValue(y, x) == 0)
                    {
                        objBattle.SetEnemyValue(y, x, 4);
                        // Обновляем очки при промахе
                        objBattle.UpdateScore(false);
                        // Обновляем отображение очков
                        labelScore.Text = "Очки: " + objBattle.PlayerScore.ToString();
                        
                        EnemyTurn();
                        panel2.Invalidate();
                    }
                }
            }
        }

        // Метод для проверки потопления корабля
        private bool CheckIfShipSunk(int x, int y, int[][] field)
        {
            // Проверяем все соседние клетки
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0) continue;
                    int newX = x + i;
                    int newY = y + j;
                    
                    if (newX >= 0 && newX < 10 && newY >= 0 && newY < 10)
                    {
                        if (field[newY][newX] == 2) // Если есть неподбитая часть корабля
                            return false;
                    }
                }
            }
            return true;
        }

        // Метод для определения размера корабля
        private int GetShipSize(int x, int y, int[][] field)
        {
            int size = 1;
            // Проверяем горизонтально
            int tempX = x - 1;
            while (tempX >= 0 && (field[y][tempX] == 2 || field[y][tempX] == 3))
            {
                size++;
                tempX--;
            }
            tempX = x + 1;
            while (tempX < 10 && (field[y][tempX] == 2 || field[y][tempX] == 3))
            {
                size++;
                tempX++;
            }
            
            // Проверяем вертикально
            int tempY = y - 1;
            while (tempY >= 0 && (field[tempY][x] == 2 || field[tempY][x] == 3))
            {
                size++;
                tempY--;
            }
            tempY = y + 1;
            while (tempY < 10 && (field[tempY][x] == 2 || field[tempY][x] == 3))
            {
                size++;
                tempY++;
            }
            return size;
        }

        private void EnemyTurn()
        {
            int x, y;
            Random r = new Random();
            bool flag = true;
            while (flag)
            {
                x = r.Next(0, 10);
                y = r.Next(0, 10);
                if (objBattle[x, y] == 0 || objBattle[x, y] == 2)
                    flag = false;
                if (objBattle[x, y] == 0)
                {
                    objBattle[x, y] = 4;
                    panel1.Invalidate();
                }
                else
                {
                    objBattle[x, y] = 3;
                    objBattle.MyShip--;
                    objBattle.Explosion(x, y, objBattle.GetLink());
                    if (objBattle.MyShip == 0)
                        Victory("Победил компьютер");
                }
            }                 
        }

        private void Victory(string str)
        {
            Victrory frmVictroryGame = new Victrory(str);
            Hide();
            frmVictroryGame.Show();
        }

        private void buttonRandom_Click(object sender, EventArgs e)
        {
            objBattle.Reset();
            int y, x, count = 10, size = 3;
            Random r = new Random();
            while (count > 0)
            {
                if (count < 10) size = 2;
                if (count < 8) size = 1;
                if (count < 5) size = 0;
                y = r.Next(0, 10);
                x = r.Next(0, 10);
                if (r.Next(0, 2) == 1)
                    objBattle.ChangeVerctical();
                if (objBattle.CheckSq(x, y, size, objBattle.GetLink()))
                {
                    objBattle.CreateShip(x, y, 2, size, objBattle.GetLink());
                    if (size == 0) objBattle.Count1X++;
                    else if (size == 1) objBattle.Count2X++;
                    else if (size == 2) objBattle.Count3X++;
                    else objBattle.Count4X++;
                    count--;
                }
            }
            panel1.Invalidate();
            UpdateShipPlacementOrder();
        }

        private void UpdateShipPlacementOrder()
        {
            // 4x корабль
            if (objBattle.Count4X < 1)
            {
                radioButton4.Enabled = true;
                radioButton4.Checked = true;
                radioButton3.Enabled = false;
                radioButton2.Enabled = false;
                radioButton1.Enabled = false;
                return;
            }
            // 3x корабли
            if (objBattle.Count3X < 2)
            {
                radioButton4.Enabled = false;
                radioButton3.Enabled = true;
                radioButton3.Checked = true;
                radioButton2.Enabled = false;
                radioButton1.Enabled = false;
                return;
            }
            // 2x корабли
            if (objBattle.Count2X < 3)
            {
                radioButton4.Enabled = false;
                radioButton3.Enabled = false;
                radioButton2.Enabled = true;
                radioButton2.Checked = true;
                radioButton1.Enabled = false;
                return;
            }
            // 1x корабли
            if (objBattle.Count1X < 4)
            {
                radioButton4.Enabled = false;
                radioButton3.Enabled = false;
                radioButton2.Enabled = false;
                radioButton1.Enabled = true;
                radioButton1.Checked = true;
                return;
            }
            // Всё размещено
            radioButton4.Enabled = false;
            radioButton3.Enabled = false;
            radioButton2.Enabled = false;
            radioButton1.Enabled = false;
            gotov = true;
        }
	}
}
