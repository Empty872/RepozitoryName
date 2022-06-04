using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Timers.Timer;
using System.Drawing;
using ContentAlignment = System.Drawing.ContentAlignment;

namespace WinFormsApp2
{
    
    public static class Upgrades
    {
        public static int LabelBottom;
        public static bool UltimateUpgradeAvailable;
        public static List<Action> possibleUpgrades;
        public static List<Action<Graphics>> obtainedUpgrades = new ();

        public static void GetFloatedBallUpgrade()
        {
            SpikedBall.Timer.Elapsed += SpikedBall.Move;
            SpikedBall.Timer.Start();
            Form1.SpikedBallLabel.Location = new Point(5, LabelBottom);
            Form1.SpikedBallLabel.Show();
            Form1.SpikedBallLabel.Update();
            LabelBottom += 120;
            possibleUpgrades.Remove(GetFloatedBallUpgrade);
            obtainedUpgrades.Add(FloatedBallUpgrade);
        }

        public static void GetUltimateUpgrade()
        {
            UltimateUpgradeAvailable = true;
            Form1.IndicatorForUltimate.Location = new Point(100, LabelBottom + 90);
            Form1.IndicatorForUltimate.Show();
            Form1.IndicatorForUltimate.Update();
            Form1.LabelForUltimate.Location = new Point(10, LabelBottom);
            Form1.LabelForUltimate.Show();
            Form1.LabelForUltimate.Update();
            LabelBottom += 120;
            possibleUpgrades.Remove(GetUltimateUpgrade);
        }
        public static void FloatedBallUpgrade(Graphics g)
        {
            
            var defeatedEnemies = new List<Enemy>();
            foreach (var enemy in Form1.Enemies)
            {
                if (enemy.ContactWithSpikedBall())
                    defeatedEnemies.Add(enemy);
            }

            foreach (var enemy in defeatedEnemies)
                Form1.Enemies.Remove(enemy);
            g.DrawImage(SpikedBall.Image, Player.CenterX+SpikedBall.X, Player.CenterY+SpikedBall.Y, 36, 36);
        }

        public static void Reset()
        {
            possibleUpgrades = new List<Action> {GetFloatedBallUpgrade, GetUltimateUpgrade};
            obtainedUpgrades.Clear();
            SpikedBall.Reset();
            UltimateUpgradeAvailable = false;
            LabelBottom = 120;
        }
    }
}