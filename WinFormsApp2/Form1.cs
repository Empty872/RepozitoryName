using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Timers;
using System.Windows.Forms;
using ContentAlignment = System.Drawing.ContentAlignment;
using Timer = System.Timers.Timer;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public static List<Blade> Blades = new();
        public static List<Enemy> Enemies = new();
        private bool GameIsOver;
        private Button StartGameButton;
        private bool GameIsStarted;
        private Button RestartButton;
        public static int DefeatedEnemiesCount;
        private int Record;
        private Timer TimerForEnemies;
        private Timer TimerForPlayer;
        public static Timer TimerForBlades;
        private Timer TimerCooldownAttack;
        public static Timer TimerCooldownUltimate;
        public static Label LabelForPanel;
        public static Label LabelForDefeatedEnemies;
        public static Label LabelForUltimate;
        public static Label IndicatorForUltimate;
        public static Label LabelForSpikedBall;
        public static Label LabelForAttack;
        public static Label LabelForSpeed;
        private SoundPlayer ThrownBladeSound;
        private SoundPlayer GameOverSound;
        private SoundPlayer SlowDownSound;
        private Label LabelForRecord;
        private Label LabelForScore;

        public static Image GrassImage = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\grass.png"));

        public static Image BushImage = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bushes.png"));


        public Form1()
        {
            ThrownBladeSound = new SoundPlayer(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Sounds\\бросокКинжала.wav"));
            GameOverSound = new SoundPlayer(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Sounds\\проигрыш.wav"));
            SlowDownSound =new SoundPlayer(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Sounds\\замедление.wav"));
            
            InitializeComponent();
            MakeButtons();
            KeyPreview = true;
            WindowState = FormWindowState.Maximized;
            KeyDown += OnPress;
            KeyUp += OnUnpress;
            MouseClick += Atack;
            MakeTimers();
            CreateLabels();
        }

        private void ShowLabels()
        {
            LabelForDefeatedEnemies.Show();
            LabelForAttack.Show();
            LabelForPanel.Show();
            LabelForDefeatedEnemies.Update();
            LabelForAttack.Update();
            LabelForPanel.Update();
        }

        private void HideLabels()
        {
            IndicatorForUltimate.Hide();
            LabelForDefeatedEnemies.Hide();
            LabelForAttack.Hide();
            LabelForUltimate.Hide();
            LabelForSpeed.Hide();
            LabelForPanel.Hide();
            LabelForSpikedBall.Hide();
        }

        private void CreateLabels()
        {
            LabelForPanel = new Label
            {
                Size = new Size(140, Height),
                BackColor = Color.SaddleBrown
            };
            LabelForDefeatedEnemies = new Label
            {
                Location = new Point(10, 0),
                Size = new Size(120, 120),
                Image = new Bitmap(Enemy.Image, 90, 72),
                BackColor = Color.SaddleBrown,
                Text = DefeatedEnemiesCount.ToString(),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight
            };
            LabelForAttack = new Label
            {
                Size = new Size(120, 120),
                Location = new Point(10, 120),
                BackColor = Color.SaddleBrown
            };
            LabelForUltimate = new Label
            {
                Size = new Size(120, 120),
                BackColor = Color.SaddleBrown,
            };
            IndicatorForUltimate = new Label
            {
                Size = new Size(20, 20),
                Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\greenCircle.png")),
                BackColor = Color.SaddleBrown
            };
            LabelForSpikedBall = new Label
            {
                Size = new Size(120, 120),
                Image = new Bitmap(new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\spikedBallLabel.png")), 120, 120),
                BackColor = Color.SaddleBrown
            };
            LabelForSpeed = new Label
            {
                Size = new Size(120, 120),
                BackColor = Color.SaddleBrown
            };
            LabelForRecord = new Label
            {
                Location = new Point(1025, 70),
                Font = new Font("Arial", 30, FontStyle.Bold),
                Size = new Size(330, 60),
                BackColor = Color.White
            };
            LabelForScore = new Label
            {
                Location = new Point(650, 70),
                Font = new Font("Arial", 30, FontStyle.Bold),
                Size = new Size(300, 60),
                BackColor = Color.White
            };

            Controls.AddRange(new[]
            {
                LabelForDefeatedEnemies, LabelForAttack, LabelForUltimate, IndicatorForUltimate, LabelForSpikedBall,
                LabelForSpeed, LabelForPanel
            });
            HideLabels();
            Controls.Add(LabelForRecord);
            Controls.Add(LabelForScore);
            LabelForRecord.Hide();
            LabelForScore.Hide();
        }

        private void MakeTimers()
        {
            TimerForEnemies = new Timer(1);
            TimerForEnemies.Elapsed += MoveEnemies;
            TimerForEnemies.Start();
            TimerForPlayer = new Timer(20);
            TimerForPlayer.Elapsed += Player.Move;
            TimerForPlayer.Start();
            TimerForBlades = new Timer(1);
            TimerForBlades.Elapsed += (sender, e) =>
            {
                foreach (var blade in Blades)
                {
                    blade.Distance += 18;
                }
            };
            TimerForBlades.Start();
            TimerCooldownAttack = new Timer(1000);
            TimerCooldownAttack.Elapsed += (sender, e) => { Player.CanAttack = true; };
        }

        public void GameOver(Graphics g)
        {
            if (GameIsStarted)
                GameOverSound.Play();
            GameIsStarted = false;
            HideLabels();
            g.Clear(Color.White);
            var gameOverImage =
                new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\GameOver.png"));
            g.DrawImage(gameOverImage,
                new Point(Width / 2 - gameOverImage.Width / 2, Height / 2 - gameOverImage.Height+30));
            RestartButton.Show();
            Record = Math.Max(Record, DefeatedEnemiesCount);
            LabelForRecord.Text = "Record: " + Record;
            LabelForScore.Text = "Score: " + DefeatedEnemiesCount;
            LabelForRecord.Update();
            LabelForScore.Update();
            LabelForRecord.Show();
            LabelForScore.Show();
        }


        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.DirUp = 3;
                    break;
                case Keys.S:
                    Player.DirDown = 3;
                    break;
                case Keys.A:
                    Player.DirLeft = 3;
                    if (!Player.IsFlipped)
                    {
                        Player.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        Player.IsFlipped = true;
                    }
                    break;
                case Keys.D:
                    Player.DirRight = 3;
                    if (Player.IsFlipped)
                    {
                        Player.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        Player.IsFlipped = false;
                    }
                    break;
            }
        }

        public void OnUnpress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    Player.DirUp = 0;
                    break;
                case Keys.S:
                    Player.DirDown = 0;
                    break;
                case Keys.A:
                    Player.DirLeft = 0;
                    if (Player.DirRight != 0 && Player.IsFlipped)
                    {
                        Player.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        Player.IsFlipped = false;
                    }
                    break;
                case Keys.D:
                    Player.DirRight = 0;
                    if (Player.DirLeft != 0 && !Player.IsFlipped)
                    {
                        Player.Image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                        Player.IsFlipped = true;
                    }
                    break;
            }
        }

        public void Atack(object sender, MouseEventArgs e)
        {
            var dY = e.Y - Player.CenterY;
            var dX = e.X - Player.CenterX;
            var angle = Math.Atan(dY / (double) dX) * 180 / Math.PI;
            if (dX < 0)
                angle += 180;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!Player.CanAttack)
                        break;
                    ThrownBladeSound.Play();
                    Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle));
                    if (Upgrades.CanThrowThreeBlades)
                    {
                        Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle - 30));
                        Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle + 30));
                    }

                    TimerCooldownAttack.Start();
                    if (Upgrades.CanThrowDoubleBlade)
                    {
                        var timer = new Timer(100);
                        timer.Elapsed += (object sender, ElapsedEventArgs e) =>
                        {
                            lock (Blades)
                                Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle));
                            timer.Stop();
                        };
                        timer.Start();
                    }

                    Player.CanAttack = false;
                    TimerCooldownAttack.Stop();
                    TimerCooldownAttack.Start();
                    break;
                case MouseButtons.Right:
                    if (!Player.UltimateIsReady)
                        return;
                    switch (Upgrades.UltimateUpgrade)
                    {
                        case "CircleOfBlades":
                            TimerCooldownUltimate.Start();
                            IndicatorForUltimate.Image = new Bitmap(Path.Combine(
                                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                "Models\\redCircle.png"));
                            IndicatorForUltimate.Update();
                            var list = new List<Blade>();
                            for (int i = -36; i < 36; i++)
                            {
                                list.Add(new Blade(Player.CenterX, Player.CenterY, angle + i * 5));
                            }

                            ThrownBladeSound.Play();
                            Blades.AddRange(list);
                            Player.UltimateIsReady = false;
                            return;
                        case "SlowDown":
                            SlowDownSound.Play();
                            TimerCooldownUltimate.Start();
                            IndicatorForUltimate.Image = new Bitmap(Path.Combine(
                                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                                "Models\\redCircle.png"));
                            IndicatorForUltimate.Update();
                            foreach (var enemy in Enemies)
                                enemy.Speed = 2f;
                            Player.UltimateIsReady = false;
                            return;
                    }
                    break;
            }
        }

        public void PaintBlades(Graphics g)
        {
            lock (Blades)
            {
                foreach (var blade in Blades)
                {
                    g.TranslateTransform(blade.PlayerCenterX, blade.PlayerCenterY);
                    g.RotateTransform((float) blade.Angle);
                    g.DrawImage(Blade.Image, blade.Distance, 0, Blade.Image.Width, Blade.Image.Height);
                    g.ResetTransform();
                }
            }
        }

        private void MoveEnemies(object sender, ElapsedEventArgs e)
        {
            foreach (var enemy in Enemies)
            {
                enemy.Move();
            }
        }

        public void PaintEnemies(Graphics g)
        {
            foreach (var enemy in Enemies)
            {
                g.DrawImage(Enemy.Image, enemy.PosX, enemy.PosY, Enemy.Image.Width, Enemy.Image.Height);
            }
        }

        private void EnemyContactBlade()
        {
            var deletedEnemies = new List<Enemy>();
            var deletedBlades = new List<Blade>();
            lock (Blades)
            {
                foreach (var blade in Blades)
                {
                    var newX = (blade.Distance + Blade.Image.Width) * Math.Cos(blade.Angle * Math.PI / 180) +
                               blade.PlayerCenterX;
                    var newY = (blade.Distance + Blade.Image.Width) * Math.Sin(blade.Angle * Math.PI / 180) +
                               blade.PlayerCenterY;
                    if (newX + 100 < 0 || newX - 100 > Width || newY + 100 < 0 || newY - 100 > Height)
                    {
                        deletedBlades.Add(blade);
                        continue;
                    }

                    foreach (var enemy in Enemies)
                    {
                        if (enemy.ContactWithBlade(blade))
                        {
                            deletedBlades.Add(blade);
                            deletedEnemies.Add(enemy);
                            DefeatedEnemiesCount++;
                            LabelForDefeatedEnemies.Text = DefeatedEnemiesCount.ToString();
                            if (DefeatedEnemiesCount % 20 == 0 && DefeatedEnemiesCount != 0 &&
                                Upgrades.possibleUpgrades.Count != 0)
                                Upgrades.possibleUpgrades[new Random().Next(Upgrades.possibleUpgrades.Count)]();
                            LabelForDefeatedEnemies.Update();

                            break;
                        }
                    }


                    foreach (var deletedEnemy in deletedEnemies)
                    {
                        Enemies.Remove(deletedEnemy);
                    }
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
                if (enemy.ContactWithPlayer())
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
            var firstTupleX = new Random().Next(LabelForPanel.Right - Enemy.Image.Width, Width);
            var firstTupleY = new Random().Next(2) == 0 ? -Enemy.Image.Height : Height;
            var firstTuple = Tuple.Create(firstTupleX, firstTupleY);
            var secondTupleX = new Random().Next(2) == 0 ? LabelForPanel.Right - Enemy.Image.Width : Width;
            var secondTupleY = new Random().Next(-Enemy.Image.Height, Height);
            var secondTuple = Tuple.Create(secondTupleX, secondTupleY);
            var finalTuple = new Random().Next(2) == 0 ? firstTuple : secondTuple;
            Enemies.Add(new Enemy(finalTuple.Item1, finalTuple.Item2));
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
                Location = new Point(Width / 2 - 150, Height / 2 + 130),
                Size = new Size(300, 150),
                Text = "Restart",
                Font = new Font("Arial", 30, FontStyle.Bold),
            };
            Controls.Add(StartGameButton);
            Controls.Add(RestartButton);
            StartGameButton.Click += (sender, e) =>
            {
                BackgroundImage = GrassImage;
                StartNewGame();
                Controls.Remove(StartGameButton);
            };
            RestartButton.Click += (sender, e) =>
            {
                RestartButton.Hide();
                StartNewGame();
            };
            RestartButton.Hide();
        }

        private void StartNewGame()
        {
            GameIsOver = false;
            GameIsStarted = true;
            Player.MakeNewPlayer(960, 450);
            Enemies.Clear();
            Blades.Clear();
            DefeatedEnemiesCount = 0;
            LabelForDefeatedEnemies.Text = DefeatedEnemiesCount.ToString();
            Upgrades.Reset();
            LabelForRecord.Hide();
            LabelForScore.Hide();
            ShowLabels();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            if (GameIsOver)
                GameOver(g);
            if (!GameIsStarted)
                return;
            DoubleBuffered = true;
            g.DrawImage(Player.Image, Player.PosX, Player.PosY, Player.ImageWidth, Player.ImageHeight);
            CreateEnemies();
            PaintEnemies(g);
            PaintBlades(g);
            foreach (var upgrade in Upgrades.obtainedUpgrades)
            {
                upgrade(g);
            }
            EnemyContactBlade();
            PlayerContactEnemy();
            g.DrawImage(BushImage, LabelForPanel.Width, 0, BushImage.Width, BushImage.Height);
            Invalidate();
        }
    }
}