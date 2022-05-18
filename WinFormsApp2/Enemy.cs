using System.Drawing;
using System.IO;

namespace WinFormsApp2
{
    public class Enemy
    {
        public int PosX;
        public int PosY;
        public int CenterPosX;
        public int CenterPosY;
        public static Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigSlime.png"));
        public int Health;

        public Enemy(int posX, int posY, int health)
        {
            PosY = posY;
            PosX = posX;
            CenterPosX = posX + Image.Width / 2;
            CenterPosY = posY + Image.Height / 2;
            Health = health;
        }

    }
}