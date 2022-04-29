using System.Drawing;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Player
    {
        public int PosX;
        public int PosY;
        public Image Image = new Bitmap(@"C:\Users\User\Desktop\WinFormsApp2\WinFormsApp2\Models\newAssassin.png");
        
        public Player(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
        
    }
}