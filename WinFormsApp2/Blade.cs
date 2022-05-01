using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Blade
    {
        public int PosX;
        public Image Image = new Bitmap(@"C:\Users\User\Desktop\WinFormsApp2\WinFormsApp2\Models\bigClearBlade.png");
        public int PlayerPosX;
        public int PlayerPosY;
        public double Angle;
        

        public Blade(int posX, int  playerPosX, int playerPosY, double angle)
        {
            PosX = posX;
            PlayerPosX = playerPosX;
            PlayerPosY = playerPosY;
            Angle = angle;
        }

    }
}