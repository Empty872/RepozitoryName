using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Blade
    {
        public int PosX;
        public int PosY;
        public Image Image = new Bitmap(@"C:\Users\User\Desktop\WinFormsApp2\WinFormsApp2\Models\bigClearBlade.png");
        

        public Blade(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
        public void Fly(Graphics g)
        {
            PosX += 5;
        }

    }
}