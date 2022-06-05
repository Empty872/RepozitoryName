using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public static class Player
    {
        public static float PosX;
        public static float PosY;
        public static float DirRight;
        public static float DirDown;
        public static float DirLeft;
        public static float DirUp;

        public static readonly Image Image = new Bitmap(Path.Combine(
            new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName,
            "Models\\newAssassin.png"));

        public static int ImageWidth = Image.Width;
        public static int ImageHeight = Image.Height;
        public static float CenterX ;
        public static float CenterY;
        public static bool CanAttack;
        public static bool UltimateIsReady;
        public static float Speed;

        

        public static void Move(object sender, EventArgs e)
        {
            if (PosX + ImageWidth < 1726+Form1.LabelForPanel.Width)
            {
                PosX += DirRight*Speed;
                CenterX += DirRight*Speed;
            }

            if (PosX >63+Form1.LabelForPanel.Width)
            {
                PosX -= DirLeft*Speed;
                CenterX -= DirLeft*Speed;
            }
            if (PosY + ImageHeight < 936)
            {
                PosY += DirDown*Speed;
                CenterY += DirDown*Speed;
            }
            if (PosY > 66)
            {
                PosY -= DirUp*Speed;
                CenterY -= DirUp*Speed;
            }
        }

        public static void MakeNewPlayer(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
            CenterX = PosX + ImageWidth / 2;
            CenterY = PosY + ImageHeight / 2;
            CanAttack = true;
            UltimateIsReady = true;
        }
    }
}