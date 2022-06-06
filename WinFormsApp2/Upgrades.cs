using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Runtime.InteropServices;
using System.Timers;

namespace WinFormsApp2
{
    public static class Upgrades
    {
        public static int LabelBottom;
        public static string UltimateUpgrade;
        public static List<Action> possibleUpgrades;
        public static List<Action<Graphics>> obtainedUpgrades = new();
        public static bool CanThrowThreeBlades;
        public static bool CanThrowDoubleBlade;

        public static void GetFloatedBallUpgrade()
        {
            SpikedBall.Timer.Elapsed += SpikedBall.Move;
            SpikedBall.Timer.Start();
            Form1.LabelForSpikedBall.Location = new Point(5, LabelBottom);
            Form1.LabelForSpikedBall.Show();
            Form1.LabelForSpikedBall.Update();
            LabelBottom += 120;
            possibleUpgrades.Remove(GetFloatedBallUpgrade);
            obtainedUpgrades.Add(FloatedBallUpgrade);
        }

        public static void GetSlowDownUltimateUpgrade()
        {
            UltimateUpgrade = "SlowDown";
            Form1.IndicatorForUltimate.Location = new Point(20, LabelBottom + 90);
            Form1.IndicatorForUltimate.Show();
            Form1.IndicatorForUltimate.Update();
            Form1.LabelForUltimate.Location = new Point(10, LabelBottom);
            Form1.LabelForUltimate.Image = new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\SlowDownIcon.png"));
            Form1.LabelForUltimate.Show();
            Form1.LabelForUltimate.Update();
            Form1.TimerCooldownUltimate = new Timer(15000);
            Form1.TimerCooldownUltimate.Elapsed += (sender, e) =>
            {
                Player.UltimateIsReady = true;
                Form1.IndicatorForUltimate.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\greenCircle.png"));
                Form1.IndicatorForUltimate.Update();
            };
            LabelBottom += 120;
            possibleUpgrades.Remove(GetCircleOfBladesUltimateUpgrade);
            possibleUpgrades.Remove(GetSlowDownUltimateUpgrade);
        }

        public static void GetCircleOfBladesUltimateUpgrade()
        {
            UltimateUpgrade = "CircleOfBlades";
            Form1.IndicatorForUltimate.Location = new Point(100, LabelBottom + 90);
            Form1.IndicatorForUltimate.Show();
            Form1.IndicatorForUltimate.Update();
            Form1.LabelForUltimate.Location = new Point(10, LabelBottom);
            Form1.LabelForUltimate.Image = new Bitmap(new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\CircleOfBlades.png")), 120, 120);
            Form1.LabelForUltimate.Show();
            Form1.LabelForUltimate.Update();
            Form1.TimerCooldownUltimate = new Timer(25000);
            Form1.TimerCooldownUltimate.Elapsed += (sender, e) =>
            {
                Player.UltimateIsReady = true;
                Form1.IndicatorForUltimate.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\greenCircle.png"));
                Form1.IndicatorForUltimate.Update();
            };
            LabelBottom += 120;
            possibleUpgrades.Remove(GetCircleOfBladesUltimateUpgrade);
            possibleUpgrades.Remove(GetSlowDownUltimateUpgrade);
        }

        public static void GetSpeedUpgrade1()
        {
            Player.Speed = 1.4f;
            Form1.LabelForSpeed.Location = new Point(10, LabelBottom);
            Form1.LabelForSpeed.Image = new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\1SpeedUpgradeIcon.png"));
            Form1.LabelForSpeed.Show();
            Form1.LabelForSpeed.Update();
            LabelBottom += 120;
            possibleUpgrades.Remove(GetSpeedUpgrade1);
            possibleUpgrades.Add(GetSpeedUpgrade2);
        }

        public static void GetSpeedUpgrade2()
        {
            Player.Speed = 1.7f;
            Form1.LabelForSpeed.Image = new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\2SpeedUpgradeIcon.png"));
            Form1.LabelForSpeed.Update();
            possibleUpgrades.Remove(GetSpeedUpgrade2);
            possibleUpgrades.Add(GetSpeedUpgrade3);
        }

        public static void GetSpeedUpgrade3()
        {
            Player.Speed = 2;
            Form1.LabelForSpeed.Image = new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\3SpeedUpgradeIcon.png"));
            Form1.LabelForSpeed.Update();
            possibleUpgrades.Remove(GetSpeedUpgrade3);
        }


        public static void GetThreeBladesUpgrade()
        {
            CanThrowThreeBlades = true;
            possibleUpgrades.Remove(GetThreeBladesUpgrade);
            if (possibleUpgrades.Contains(GetDoubleBladeUpgrade))
                Form1.LabelForAttack.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\3BladeIcon.png"));
            else
                Form1.LabelForAttack.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\3BladeX2Icon.png"));
            Form1.LabelForAttack.Show();
            Form1.LabelForAttack.Update();
        }

        public static void GetDoubleBladeUpgrade()
        {
            CanThrowDoubleBlade = true;
            possibleUpgrades.Remove(GetDoubleBladeUpgrade);
            if (possibleUpgrades.Contains(GetThreeBladesUpgrade))
                Form1.LabelForAttack.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\1BladeX2Icon.png"));
            else
                Form1.LabelForAttack.Image = new Bitmap(Path.Combine(
                    new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                    "Models\\3BladeX2Icon.png"));
            Form1.LabelForAttack.Update();
        }

        public static void FloatedBallUpgrade(Graphics g)
        {
            var defeatedEnemies = new List<Enemy>();
            foreach (var enemy in Form1.Enemies)
            {
                if (enemy.ContactWithSpikedBall())
                {
                    defeatedEnemies.Add(enemy);
                    Form1.DefeatedEnemiesCount++;
                    Form1.LabelForDefeatedEnemies.Text = Form1.DefeatedEnemiesCount.ToString();
                    Form1.LabelForDefeatedEnemies.Update();
                    if (Form1.DefeatedEnemiesCount % 20 == 0 && Form1.DefeatedEnemiesCount != 0 &&
                        possibleUpgrades.Count != 0)
                        possibleUpgrades[new Random().Next(possibleUpgrades.Count)]();
                }
            }

            foreach (var enemy in defeatedEnemies)
                Form1.Enemies.Remove(enemy);
            g.DrawImage(SpikedBall.Image, Player.CenterX + SpikedBall.X, Player.CenterY + SpikedBall.Y, 36, 36);
        }

        public static void Reset()
        {
            possibleUpgrades = new List<Action>
            {
                GetFloatedBallUpgrade, GetCircleOfBladesUltimateUpgrade, GetSlowDownUltimateUpgrade, GetSpeedUpgrade1,
                GetThreeBladesUpgrade, GetDoubleBladeUpgrade
            };
            obtainedUpgrades.Clear();
            SpikedBall.Reset();
            UltimateUpgrade = "";
            LabelBottom = 240;
            Player.Speed = 1;
            CanThrowThreeBlades = false;
            CanThrowDoubleBlade = false;
            Form1.LabelForAttack.Image = new Bitmap(Path.Combine(
                new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
                "Models\\1BladeIcon.png"));
        }
    }
}