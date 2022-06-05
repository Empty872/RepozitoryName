using System;
using System.Drawing;
using System.IO;

namespace WinFormsApp2
{
    public class Enemy
    {
        public float PosX;
        public float PosY;
        public float CenterPosX;
        public float CenterPosY;
        public static readonly Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigSlime.png"));

        public Enemy (float posX, float posY)
        {
            PosY = posY;
            PosX = posX;
            CenterPosX = posX + Image.Width / 2;
            CenterPosY = posY + Image.Height / 2;
        }

        public bool ContactWithSomething(float left, float right, float top, float bottom)
        {
            return Math.Max(PosX, left) <= Math.Min(PosX + Image.Width, right) &&
                    Math.Max(PosY, top) <= Math.Min(PosY + Image.Height, bottom);
        }
        public bool ContactWithBlade(Blade blade)
        {
            var firstX = (blade.Distance + Blade.Image.Width) * Math.Cos(blade.Angle * Math.PI / 180) +
                       blade.PlayerCenterX;
            var firstY = (blade.Distance + Blade.Image.Width) * Math.Sin(blade.Angle * Math.PI / 180) +
                       blade.PlayerCenterY;
            var secondX = firstX+Blade.Image.Height*Math.Sin(blade.Angle*Math.PI/180);
            var secondY = firstY + Blade.Image.Height*Math.Cos(blade.Angle*Math.PI/180);

            return (firstX > PosX && firstX < PosX + Image.Width && firstY > PosY && firstY < PosY + Image.Height) ||
                    (secondX > PosX && secondX < PosX + Image.Width && secondY > PosY && secondY < PosY + Image.Height);
        }
        public bool ContactWithPlayer()
        {
            return ContactWithSomething(Player.PosX, Player.PosX + Player.ImageWidth, Player.PosY,
                Player.PosY + Player.ImageHeight);
        }
        public bool ContactWithSpikedBall()
        {
            var newX = SpikedBall.X + Player.CenterX;
            var newY = SpikedBall.Y + Player.CenterY;
            return ContactWithSomething(newX, newX + 36, newY,
                newY + 36);
        }

    }
}