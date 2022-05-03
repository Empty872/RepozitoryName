using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Player Player;
        public Enemy NoneEnemy = new (0, 0);
        private List<Blade> Blades = new();
        private List<Enemy> Enemies = new();
        private bool GameIsOver;

        public Form1()
        {
            
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            Invalidate();
            Player = new Player(900, 400);
            KeyDown += OnPress;
            MouseClick += ThrowTheBlade;
        }

        public void GameOver(Graphics g)
        {
            if (GameIsOver)
            {
                g.Clear(Color.White);
                var gameOverImage=new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\GameOver.png"));
                g.DrawImage(gameOverImage, new Point(Width/2-gameOverImage.Width/2, Height/2-gameOverImage.Height/2));
            }
        }
        
        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.PosY -= 5;
                    Player.CenterPosY -= 5;
                    break;
                case Keys.S:
                    Player.PosY += 5;
                    Player.CenterPosY += 5;
                    break;
                case Keys.A:
                    Player.PosX -= 5;
                    Player.CenterPosX -= 5;
                    break;
                case Keys.D:
                    Player.PosX += 5;
                    Player.CenterPosX += 5;
                    break;
            }

            Invalidate();
        }

        public void ThrowTheBlade(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    var dY = e.Y - Player.CenterPosY;
                    var dX = e.X - Player.CenterPosX;
                    var angle = Math.Atan((double) dY / (double) dX) * 180 / Math.PI;
                    if (dX < 0)
                        angle += 180;
                    var knife = new Blade(Player.CenterPosX, Player.CenterPosY, angle);
                    Blades.Add(knife);
                    break;
            }
            Invalidate();
        }

        public void PaintBlades(Graphics g)
        {
            foreach (var blade in Blades)
            {
                g.TranslateTransform(blade.PlayerCenterX, blade.PlayerCenterY);
                g.RotateTransform((float) blade.Angle);
                blade.Distance += 5;
                g.DrawImage(blade.Image, blade.Distance, 0, blade.Image.Width, blade.Image.Height);
                g.ResetTransform();
                Invalidate();
            }
        }

        public void PaintEnemies(Graphics g)
        {
            foreach (var enemy in Enemies)
            {
                var dirX = Player.CenterPosX-enemy.CenterPosX;
                var dirY = Player.CenterPosY-enemy.CenterPosY;
                enemy.PosX += Math.Sign(dirX);
                enemy.PosY += Math.Sign(dirY);
                enemy.CenterPosX += Math.Sign(dirX);
                enemy.CenterPosY += Math.Sign(dirY);
                g.DrawImage(enemy.Image, enemy.PosX, enemy.PosY, enemy.Image.Width, enemy.Image.Height);
                Invalidate();
            }
        }

        private void EnemyContactBlade()
        {
            var deletedEnemies = new List<Enemy>();
            var deletedBlades = new List<Blade>();
            foreach (var blade in Blades)
            {
                foreach (var enemy in Enemies)
                {
                    var newX = (blade.Distance + blade.Image.Width) * Math.Cos(blade.Angle * Math.PI / 180) + blade.PlayerCenterX;
                    var newY = (blade.Distance + blade.Image.Width) * Math.Sin(blade.Angle * Math.PI / 180) + blade.PlayerCenterY;
                    if (newX <= enemy.PosX + enemy.Image.Width && newX >= enemy.PosX &&
                        newY <= enemy.PosY + enemy.Image.Height && newY >= enemy.PosY)
                    {
                        deletedBlades.Add(blade);
                        deletedEnemies.Add(enemy);
                        break;
                    }
                }
            }

            foreach (var blade in deletedBlades)
            {
                Blades.Remove(blade);
            }

            foreach (var enemy in deletedEnemies)
            {
                Enemies.Remove(enemy);
            }
        }

        private void PlayerContactEnemy()
        {
            foreach (var enemy in Enemies)
            {
                if ((enemy.PosX >= Player.PosX && enemy.PosX <= Player.PosX + Player.Image.Width ||
                     enemy.PosX + enemy.Image.Width >= Player.PosX &&
                     enemy.PosX + enemy.Image.Width <= Player.PosX + Player.Image.Width) &&
                    (enemy.PosY >= Player.PosY && enemy.PosY <= Player.PosY + Player.Image.Height ||
                     enemy.PosY + enemy.Image.Height >= Player.PosY &&
                     enemy.PosY + enemy.Image.Height <= Player.PosY + Player.Image.Height))
                {
                    GameIsOver = true;
                    break;
                }
            }
        }

        public void CreateEnemy()
        {
            var firstTupleX = new Random().Next(-NoneEnemy.Image.Width, Width + NoneEnemy.Image.Width);
            var firstTupleY = new Random().Next(2) == 0 ? -NoneEnemy.Image.Height : Height + NoneEnemy.Image.Height;
            var firstTuple = Tuple.Create(firstTupleX, firstTupleY);
            var secondTupleX = new Random().Next(2) == 0 ? -NoneEnemy.Image.Width : Width + NoneEnemy.Image.Width;
            var secondTupleY = new Random().Next(-NoneEnemy.Image.Height, Height + NoneEnemy.Image.Height);
            var secondTuple = Tuple.Create(secondTupleX, secondTupleY);
            var finalTuple = new Random().Next(2) == 0 ? firstTuple : secondTuple;
            Enemies.Add(new Enemy(finalTuple.Item1, finalTuple.Item2));
        }
        
        protected override void OnPaint(PaintEventArgs e)
        {
            
            Graphics g = e.Graphics;
            DoubleBuffered = true;
            g.DrawImage(Player.Image, Player.PosX, Player.PosY, Player.Image.Width, Player.Image.Height);
            if (Enemies.Count<4)
                CreateEnemy();
            PaintEnemies(g);
            PaintBlades(g);
            EnemyContactBlade();
            PlayerContactEnemy();
            GameOver(g);
        }
    }
}