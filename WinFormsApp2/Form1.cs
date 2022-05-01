using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.VisualBasic.Devices;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Player player;

        private List<Blade> knifes = new();

        public Form1()
        {
            InitializeComponent();
            player = new Player(200, 200);
            Invalidate();
            WindowState = FormWindowState.Maximized;
            KeyDown += OnPress;
            MouseClick += ThrowTheBlade;
        }

        public void OnPress(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.W:
                    player.PosY -= 5;
                    break;
                case Keys.S:
                    player.PosY += 5;
                    break;
                case Keys.A:
                    player.PosX -= 5;
                    break;
                case Keys.D:
                    player.PosX += 5;
                    break;
            }

            Invalidate();
        }

        public void ThrowTheBlade(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:
                    var playerPosX = player.PosX + player.Image.Width / 2;
                    var playerPosY = player.PosY + player.Image.Height / 2;
                    var dY = e.Y - playerPosY;
                    var dX = e.X - playerPosX;
                    var tan = (double) dY / (double) dX;
                    var angle = Math.Atan(tan) * 180 / Math.PI;
                    if (dX < 0)
                        angle += 180;
                    var knife = new Blade(playerPosX, playerPosY, angle);
                    knifes.Add(knife);
                    break;
            }
            Invalidate();
        }

        public void PaintKnifes(Graphics g)
        {
            foreach (var knife in knifes)
            {
                g.TranslateTransform(knife.PlayerPosX, knife.PlayerPosY);
                g.RotateTransform((float) knife.Angle);
                knife.PosX += 5;
                g.DrawImage(knife.Image,
                    new Rectangle(new Point(knife.PosX, 0), new Size(44, 14)),
                    0, 0, 44, 14, GraphicsUnit.Pixel);
                g.ResetTransform();
                Invalidate();
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DoubleBuffered = true;
            g.DrawImage(player.Image, new Rectangle(new Point(player.PosX, player.PosY), new Size(64, 92)), 0, 0, 64,
                92, GraphicsUnit.Pixel);
            PaintKnifes(g);
        }
    }
}