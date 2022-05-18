using System;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Player
    {
        public int PosX;
        public int PosY;
        public int DirX;
        public int DirY;
        public static Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\newAssassin.png"));
        public int CenterX;
        public int CenterY;
        public bool IsMoving;
        public bool CanAttack = true;
        public bool UltimateIsReady = true;

        public Player(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
            CenterX = posX + Image.Width / 2;
            CenterY = posY + Image.Height / 2;
        }

        public void Move(object sender, EventArgs e)
        {
            if (!IsMoving)
                return;
            PosX += DirX;
            CenterX += DirX;
            PosY += DirY;
            CenterY += DirY;
        }

        public void MakeNewPlayer(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
            CenterX = PosX + Image.Width / 2;
            CenterY = PosY + Image.Height / 2;
            CanAttack = true;
            UltimateIsReady = true;
        }
    }
}