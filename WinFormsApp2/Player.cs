using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Player
    {
        public static float PosX;
        public static float PosY;
        public static float DirX;
        public static float DirY;
        public static Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\newAssassin.png"));
        public static float CenterX;
        public static float CenterY;
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