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

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Player player;
        public bool shoot;
        public double xCenter;
        public double yCenter;
        public double angle;

        private List<Blade> knifes = new(){new Blade(100, 100)};
        public Form1()
        {
            InitializeComponent();
            player = new Player(200, 200);
            WindowState = FormWindowState.Maximized;
            KeyDown += OnPress;
            MouseClick+=ThrowTheBlade;
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
                    xCenter = player.PosX + player.Image.Width / 2;
                    yCenter = player.PosY + player.Image.Height / 2;
                    angle = Math.Atan(e.Y - yCenter) /
                                (e.X - xCenter);
                    shoot = true;
                    // Paint += (sender, args) =>
                    // {
                    //     args.Graphics.TranslateTransform(xCenter, yCenter);
                    //     args.Graphics.RotateTransform((float) -angle);
                    //     var knife = new Blade(xCenter, yCenter);
                    //     args.Graphics.DrawImage(knife.Image,
                    //         new Rectangle(new Point(knife.PosX, knife.PosY), new Size(44, 14)),
                    //         0, 0, 44, 14, GraphicsUnit.Pixel);
                    //     knifes.Add(knife);
                    //     args.Graphics.ResetTransform();
                    // };
                    break;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            DoubleBuffered = true;
            g.DrawImage(player.Image, new Rectangle(new Point(player.PosX , player.PosY), new Size(64, 92)), 0, 0, 64, 92, GraphicsUnit.Pixel);
            if (shoot)
            {
                g.TranslateTransform((float)xCenter, (float)yCenter);
                g.RotateTransform((float)(angle*180/Math.PI));
                var knife = new Blade((int)xCenter, (int)yCenter);
                g.DrawImage(knife.Image,
                    new Rectangle(new Point(0, 0), new Size(44, 14)),
                    0, 0, 44, 14, GraphicsUnit.Pixel);
                knifes.Add(knife);
                g.ResetTransform();
                shoot = false;
            }
            foreach (var knife in knifes)
            {
                knife.PosX += 5;
                g.DrawImage(knife.Image,
                    new Rectangle(new Point(knife.PosX, knife.PosY), new Size(44, 14)),
                    0, 0, 44, 14, GraphicsUnit.Pixel);
                Invalidate();
            }
            
        }
        
    }
}