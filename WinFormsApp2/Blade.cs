using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Blade
    {
        public int Distance;
        public Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigClearBlade.png"));
        public int PlayerCenterX;
        public int PlayerCenterY;
        public double Angle;
        public static List<int> AdditionalBlades = new List<int>(){0};
        

        public Blade(int playerCenterX, int playerCenterY, double angle)
        {
            PlayerCenterX = playerCenterX;
            PlayerCenterY = playerCenterY;
            Angle = angle;
        }

    }
}