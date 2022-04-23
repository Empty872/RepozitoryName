using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public partial class Form1 : Form
    {
        public Image ship;
        public Player player;
        
        public Form1()
        {
            BackColor=Color.Aquamarine;
            InitializeComponent();
            // Initialise();
        }
        // public void Initialise()
        // {
        //     ship =
        //         new Bitmap(@"C:\Users\User\Desktop\WinFormsApp1\WinFormsApp1\Ships\корабль.png");
        //      ship = new Bitmap(ship, 100, 100);
        //     var ship = new Ship(100, 100, pirateShip);
        //     timer1.Start();
        // }
        
    }
}