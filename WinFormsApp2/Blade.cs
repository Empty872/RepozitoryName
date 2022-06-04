using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Blade
    {
        public int Distance;
        public static Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigClearBlade.png"));
        public float PlayerCenterX;
        public float PlayerCenterY;
        public double Angle;
        

        public Blade(float playerCenterX, float playerCenterY, double angle)
        {
            PlayerCenterX = playerCenterX;
            PlayerCenterY = playerCenterY;
            Angle = angle;
        }

    }
}