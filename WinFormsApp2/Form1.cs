using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.IO;
using System.Timers;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        private Player Player;
        private List<Blade> Blades = new();
        private List<Enemy> Enemies = new();
        private bool GameIsOver;
        private Button StartGameButton;
        private bool GameIsStarted;
        private Button RestartButton;
        private int DefeatedEnemiesCount;
        private System.Timers.Timer TimerForEnemies;
        private System.Timers.Timer TimerForUlta;
        private System.Timers.Timer TimerForPlayer;
        private System.Timers.Timer TimerForBlades;
        private System.Timers.Timer TimerCooldownAtack;
        private System.Timers.Timer TimerCooldownUltimate;
        private Label newLabel;


        public Form1()
        {
            InitializeComponent();
            MakeButtons();
            KeyPreview = true;
            WindowState = FormWindowState.Maximized;
            Player = new Player(960, 450);
            KeyDown += OnPress;
            KeyUp += OnUnpress;
            MouseClick += ThrowTheBlade;
            MakeTimers();
            
            var label = new Label
            {
                Size = new Size(140, Height),
                BackColor = Color.SaddleBrown
            };
            newLabel = new Label
            {
                Location = new Point(20, 20),
                Size = new Size(100, 100),
                Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigSlime.png")),
                BackColor = Color.SaddleBrown,
                Text = DefeatedEnemiesCount.ToString(),
                TextAlign = ContentAlignment.BottomRight

            };
            Controls.Add(newLabel);
            Controls.Add(label);
        }

        private void MakeTimers()
        {
            TimerForEnemies = new System.Timers.Timer();
            TimerForEnemies.Interval = 1;
            TimerForEnemies.Elapsed += MoveEnemies;
            TimerForEnemies.Start();
            TimerForPlayer = new System.Timers.Timer();
            TimerForPlayer.Interval = 20;
            TimerForPlayer.Elapsed += Player.Move;
            TimerForPlayer.Start();
            TimerForBlades = new System.Timers.Timer();
            TimerForBlades.Interval = 1;
            TimerForBlades.Elapsed += (sender, e) =>
            {
                foreach (var blade in Blades)
                {
                    blade.Distance += 18;
                }
            };
            TimerForBlades.Start();
            TimerCooldownAtack = new System.Timers.Timer();
            TimerCooldownAtack.Interval = 1000;
            TimerCooldownAtack.Elapsed += (sender, e) => { Player.CanAttack = true; };
            TimerCooldownAtack.Start();
            TimerCooldownUltimate = new System.Timers.Timer();
            TimerCooldownUltimate.Interval = 10000;
            TimerCooldownUltimate.Elapsed += (sender, e) => { Player.UltimateIsReady = true; };
            TimerCooldownUltimate.Start();
        }

        public void GameOver(Graphics g)
        {
            GameIsStarted = false;
            Player.MakeNewPlayer(960, 450);
            Enemies.Clear();
            Blades.Clear();
            Blade.AdditionalBlades = new List<int> {0};
            g.Clear(Color.White);
            var gameOverImage =
                new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\GameOver.png"));
            g.DrawImage(gameOverImage,
                new Point(Width / 2 - gameOverImage.Width / 2, Height / 2 - gameOverImage.Height));
            RestartButton.Show();
        }


        public void OnPress(object sender, KeyEventArgs e)
        {
            Player.IsMoving = true;
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.DirY = -3;
                    break;
                case Keys.S:
                    Player.DirY = 3;
                    break;
                case Keys.A:
                    Player.DirX = -3;
                    break;
                case Keys.D:
                    Player.DirX = 3;
                    break;
            }

            Invalidate();
        }

        public void OnUnpress(object sender, KeyEventArgs e)
        {
            Player.DirX = 0;
            Player.DirY = 0;
            Player.IsMoving = false;
            Invalidate();
        }

        public void ThrowTheBlade(object sender, MouseEventArgs e)
        {
            var dY = e.Y - Player.CenterY;
            var dX = e.X - Player.CenterX;
            var angle = Math.Atan((double) dY / (double) dX) * 180 / Math.PI;
            if (dX < 0)
                angle += 180;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!Player.CanAttack)
                        break;
                    foreach (var element in Blade.AdditionalBlades)
                    {
                        Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle + 90 * element));
                    }
                    Player.CanAttack = false;
                    break;
                case MouseButtons.Right:
                    if (!Player.UltimateIsReady)
                        return;
                    var list = new List<Blade>();
                    for (int i = -36; i < 36; i++)
                    {
                        list.Add(new Blade(Player.CenterX, Player.CenterY, angle + i * 5));
                    }

                    Blades.AddRange(list);
                    Player.UltimateIsReady = false;
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
                g.DrawImage(blade.Image, blade.Distance, 0, blade.Image.Width, blade.Image.Height);
                g.ResetTransform();
            }

            Invalidate();
        }

        private void MoveEnemies(object sender, ElapsedEventArgs e)
        {
            foreach (var enemy in Enemies)
            {
                var dirX = Player.CenterX - enemy.CenterPosX;
                var dirY = Player.CenterY - enemy.CenterPosY;
                enemy.PosX += 3 * Math.Sign(dirX);
                enemy.PosY += 3 * Math.Sign(dirY);
                enemy.CenterPosX += 3 * Math.Sign(dirX);
                enemy.CenterPosY += 3 * Math.Sign(dirY);
            }

            Invalidate();
        }

        public void PaintEnemies(Graphics g)
        {
            foreach (var enemy in Enemies)
            {
                g.DrawImage(Enemy.Image, enemy.PosX, enemy.PosY, Enemy.Image.Width, Enemy.Image.Height);
            }

            Invalidate();
        }

        private void EnemyContactBlade()
        {
            var deletedEnemies = new List<Enemy>();
            var deletedBlades = new List<Blade>();
            foreach (var blade in Blades)
            {
                var newX = (blade.Distance + blade.Image.Width) * Math.Cos(blade.Angle * Math.PI / 180) +
                           blade.PlayerCenterX;
                var newY = (blade.Distance + blade.Image.Width) * Math.Sin(blade.Angle * Math.PI / 180) +
                           blade.PlayerCenterY;
                if (newX + 100 < 0 || newX - 100 > Width || newY + 100 < 0 || newY - 100 > Height)
                {
                    deletedBlades.Add(blade);
                    continue;
                }
                
                foreach (var enemy in Enemies)
                {
                    if (newX <= enemy.PosX + Enemy.Image.Width && newX >= enemy.PosX &&
                        newY <= enemy.PosY + Enemy.Image.Height && newY >= enemy.PosY)
                    {
                        enemy.Health--;
                        deletedBlades.Add(blade);
                        if (enemy.Health == 0)
                        {
                            if (DefeatedEnemiesCount != 0 && DefeatedEnemiesCount % 10 == 0)
                            {
                                var newBlade = new Random().Next(0, 4);
                                Blade.AdditionalBlades.Add(newBlade);
                            }
                            deletedEnemies.Add(enemy);
                            DefeatedEnemiesCount++;
                            newLabel.Text = DefeatedEnemiesCount.ToString();
                            newLabel.Update();
                        } 
                        break;
                    }
                }
                

                foreach (var deletedEnemy in deletedEnemies)
                {
                    Enemies.Remove(deletedEnemy);
                }
            }

            foreach (var blade in deletedBlades)
            {
                Blades.Remove(blade);
            }
        }

        private void PlayerContactEnemy()
        {
            foreach (var enemy in Enemies)
            {
                if ((enemy.PosX >= Player.PosX && enemy.PosX <= Player.PosX + Player.Image.Width ||
                     enemy.PosX + Enemy.Image.Width >= Player.PosX &&
                     enemy.PosX + Enemy.Image.Width <= Player.PosX + Player.Image.Width) &&
                    (enemy.PosY >= Player.PosY && enemy.PosY <= Player.PosY + Player.Image.Height ||
                     enemy.PosY + Enemy.Image.Height >= Player.PosY &&
                     enemy.PosY + Enemy.Image.Height <= Player.PosY + Player.Image.Height))
                {
                    GameIsOver = true;
                    break;
                }
            }
        }

        public void CreateEnemies()
        {
            if (Enemies.Count >= DefeatedEnemiesCount / (5 * Enemies.Count + 1) + 1)
                return;
            var firstTupleX = new Random().Next(-Enemy.Image.Width, Width);
            var firstTupleY = new Random().Next(2) == 0 ? -Enemy.Image.Height : Height;
            var firstTuple = Tuple.Create(firstTupleX, firstTupleY);
            var secondTupleX = new Random().Next(2) == 0 ? -Enemy.Image.Width : Width;
            var secondTupleY = new Random().Next(-Enemy.Image.Height, Height);
            var secondTuple = Tuple.Create(secondTupleX, secondTupleY);
            var finalTuple = new Random().Next(2) == 0 ? firstTuple : secondTuple;
            Enemies.Add(new Enemy(finalTuple.Item1, finalTuple.Item2, 1));
        }

        public void MakeButtons()
        {
            StartGameButton = new Button
            {
                Location = new Point(Width / 2 - 150, Height / 2 - 300),
                Size = new Size(300, 150),
                Text = "Start The Game",
                Font = new Font("Arial", 30, FontStyle.Bold)
            };
            RestartButton = new Button
            {
                Location = new Point(Width / 2 - 150, Height / 2 + 100),
                Size = new Size(300, 150),
                Text = "Restart",
                Font = new Font("Arial", 30, FontStyle.Bold),
            };
            Controls.Add(StartGameButton);
            Controls.Add(RestartButton);
            StartGameButton.Click += (sender, e) =>
            {
                GameIsStarted = true;
                Controls.Remove(StartGameButton);
            };
            RestartButton.Click += (sender, e) =>
            {
                RestartButton.Hide();
                GameIsOver = false;
                GameIsStarted = true;
                DefeatedEnemiesCount = 0;
                newLabel.Text = DefeatedEnemiesCount.ToString();
                newLabel.Update();
                KeyPreview = true;
            };
            RestartButton.Hide();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (GameIsOver)
                GameOver(g);
            if (!GameIsStarted)
                return;
            DoubleBuffered = true;
            g.DrawImage(Player.Image, Player.PosX, Player.PosY, Player.Image.Width, Player.Image.Height);
            CreateEnemies();
            PaintEnemies(g);
            PaintBlades(g);
            EnemyContactBlade();
            PlayerContactEnemy();
        }
    }
}