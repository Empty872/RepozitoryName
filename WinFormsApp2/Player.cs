﻿using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Player
    {
        public int PosX;
        public int PosY;
        public Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\newAssassin.png"));
        public int CenterPosX;
        public int CenterPosY;
        
        public Player(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
            CenterPosX = posX + Image.Width / 2;
            CenterPosY = posY + Image.Height / 2;
        }
        
    }
}