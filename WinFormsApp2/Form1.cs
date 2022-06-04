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
    public partial class  Form1 : Form
    {
        public Player Player;
        public List<Blade> Blades = new();
        public static List<Enemy> Enemies = new();
        private bool GameIsOver;
        private Button StartGameButton;
        private bool GameIsStarted;
        private Button RestartButton;
        private int DefeatedEnemiesCount;
        private int Record;
        private Timer TimerForEnemies;
        private Timer TimerForPlayer;
        public static Timer TimerForBlades;
        private Timer TimerCooldownAtack;
        private Timer TimerCooldownUltimate;
        public static Label LabelForPanel;
        public static Label LabelForDefeatedEnemies;
        public static Label LabelForUltimate;
        public static Label IndicatorForUltimate;
        public static Label SpikedBallLabel;
        private SoundPlayer ThrownBladeSound;
        private SoundPlayer GameOverSound;
        private Label LabelForRecord;
        private Label LabelForScore;


        public Form1()
        {
            ThrownBladeSound = new SoundPlayer(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Sounds\\бросокКинжала.wav"));
            GameOverSound = new SoundPlayer(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Sounds\\проигрыш.wav"));
            InitializeComponent();
            MakeButtons();
            KeyPreview = true;
            WindowState = FormWindowState.Maximized;
            Player = new Player(960, 450);
            KeyDown += OnPress;
            KeyUp += OnUnpress;
            MouseClick += ThrowTheBlade;
            MakeTimers();
            CreateLabels();
        }

        private void ShowLabels()
        {
            LabelForDefeatedEnemies.Show();
            LabelForPanel.Show();
            LabelForDefeatedEnemies.Update();
            LabelForPanel.Update();
        }
        private void HideLabels()
        {
            IndicatorForUltimate.Hide();
            LabelForDefeatedEnemies.Hide();
            LabelForUltimate.Hide();
            LabelForPanel.Hide();
            SpikedBallLabel.Hide();
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
                Location = new Point(10, 10),
                Size = new Size(120, 120),
                Image = new Bitmap(Enemy.Image, 90, 72),
                BackColor = Color.SaddleBrown,
                Text = DefeatedEnemiesCount.ToString(),
                Font = new Font("Arial", 12, FontStyle.Bold),
                TextAlign = ContentAlignment.BottomRight
            };
            LabelForUltimate = new Label
            {
                Size = new Size(120, 120),
                Image = new Bitmap(new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\need.png")), 120, 120),
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
            SpikedBallLabel = new Label
            {
                Size = new Size(120, 120),
                Image = new Bitmap(new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\spikedBallLabel.png")), 120, 120),
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
            
            Controls.AddRange(new []{LabelForDefeatedEnemies, LabelForUltimate, IndicatorForUltimate, SpikedBallLabel, LabelForPanel});
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
            TimerCooldownAtack = new Timer(1000);
            TimerCooldownAtack.Elapsed += (sender, e) => { Player.CanAttack = true; };
            TimerCooldownUltimate = new Timer();
            TimerCooldownUltimate.Interval = 15000;
            TimerCooldownUltimate.Elapsed += (sender, e) =>
            {
                Player.UltimateIsReady = true;
                IndicatorForUltimate.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\greenCircle.png"));
                IndicatorForUltimate.Update();
            };
        }

        public void GameOver(Graphics g)
        {
            if (GameIsStarted)
                //GameOverSound.Play();
            GameIsStarted = false;
            HideLabels();
            g.Clear(Color.White);
            var gameOverImage =
                new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\GameOver.png"));
            g.DrawImage(gameOverImage,
                new Point(Width / 2 - gameOverImage.Width / 2, Height / 2 - gameOverImage.Height));
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
        }

        public void OnUnpress(object sender, KeyEventArgs e)
        {
            Player.DirX = 0;
            Player.DirY = 0;
            Player.IsMoving = false;
        }

        public void ThrowTheBlade(object sender, MouseEventArgs e)
        {
            var dY = e.Y - Player.CenterY;
            var dX = e.X - Player.CenterX;
            var angle = Math.Atan( dY / (double) dX) * 180 / Math.PI;
            if (dX < 0)
                angle += 180;
            switch (e.Button)
            {
                case MouseButtons.Left:
                    if (!Player.CanAttack)
                        break;
                    TimerCooldownAtack.Start();
                    ThrownBladeSound.Play();
                    Blades.Add(new Blade(Player.CenterX, Player.CenterY, angle));
                    Player.CanAttack = false;
                    TimerCooldownAtack.Start();
                    break;
                case MouseButtons.Right:
                    if (!Upgrades.UltimateUpgradeAvailable)
                        break;
                    if (!Player.UltimateIsReady)
                        return;
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
                    break;
            }
        }

        public void PaintBlades(Graphics g)
        {
            foreach (var blade in Blades)
            {
                g.TranslateTransform(blade.PlayerCenterX, blade.PlayerCenterY);
                g.RotateTransform((float) blade.Angle);
                g.DrawImage(Blade.Image, blade.Distance, 0, Blade.Image.Width, Blade.Image.Height);
                g.ResetTransform();
            }
        }

        private void MoveEnemies(object sender, ElapsedEventArgs e)
        {
            foreach (var enemy in Enemies)
            {
                var dirX = Player.CenterX - enemy.CenterPosX;
                var dirY = Player.CenterY - enemy.CenterPosY;
                enemy.PosX += dirX/(Math.Abs(dirX)+Math.Abs(dirY))*4f;
                enemy.PosY += dirY/(Math.Abs(dirX)+Math.Abs(dirY))*4f;
                enemy.CenterPosX += dirX/(Math.Abs(dirX)+Math.Abs(dirY))*4f;
                enemy.CenterPosY += dirY/(Math.Abs(dirX)+Math.Abs(dirY))*4f;
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
                        if (DefeatedEnemiesCount % 20 == 0 && DefeatedEnemiesCount!=0 && Upgrades.possibleUpgrades.Count!=0)
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
            var firstTupleX = new Random().Next(-Enemy.Image.Width, Width);
            var firstTupleY = new Random().Next(2) == 0 ? -Enemy.Image.Height : Height;
            var firstTuple = Tuple.Create(firstTupleX, firstTupleY);
            var secondTupleX = new Random().Next(2) == 0 ? -Enemy.Image.Width : Width;
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
                Location = new Point(Width / 2 - 150, Height / 2 + 100),
                Size = new Size(300, 150),
                Text = "Restart",
                Font = new Font("Arial", 30, FontStyle.Bold),
            };
            Controls.Add(StartGameButton);
            Controls.Add(RestartButton);
            StartGameButton.Click += (sender, e) =>
            {
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
            g.DrawImage(Player.Image, Player.PosX, Player.PosY, Player.Image.Width, Player.Image.Height);
            CreateEnemies();
            PaintEnemies(g);
            PaintBlades(g);
            foreach (var upgrade in Upgrades.obtainedUpgrades)
            {
                upgrade(g);
            }
            EnemyContactBlade();
            PlayerContactEnemy();
            Invalidate();
        }
    }
}