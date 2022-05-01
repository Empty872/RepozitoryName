using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Blade
    {
        public int PosX;
        public Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\bigClearBlade.png"));
        public int PlayerPosX;
        public int PlayerPosY;
        public double Angle;
        

        public Blade(int  playerPosX, int playerPosY, double angle)
        {
            PlayerPosX = playerPosX;
            PlayerPosY = playerPosY;
            Angle = angle;
        }

    }
}