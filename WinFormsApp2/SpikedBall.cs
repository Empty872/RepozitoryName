using System;
using System.Drawing;
using System.IO;
using System.Timers;

namespace WinFormsApp2
{
    public static class SpikedBall
    {
        public static float X = -118;
        public static float Y = -18;
        public static double Angle = Math.PI;
        public static Timer Timer = new (2);
        public static Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\spikedBall.png"));

        public static void Move(object sender, ElapsedEventArgs e)
        {
            Angle += Math.PI / 100;
            X = (float)Math.Cos(Angle) * 100 - 18;
            Y = (float)Math.Sin(Angle) * 100 - 18;
        }

        public static void Reset()
        {
            Timer.Elapsed -= Move;
            X = -118;
            Y = -18;
            
        }
    }
}