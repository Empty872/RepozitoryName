using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace WinFormsApp2
{
    public class Player
    {
        public int PosX;
        public int PosY;
        public Image Image = new Bitmap(Path.Combine(new DirectoryInfo(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, "Models\\newAssassin.png"));
        
        public Player(int posX, int posY)
        {
            PosX = posX;
            PosY = posY;
        }
        
    }
}