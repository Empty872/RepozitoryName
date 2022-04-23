using System.Drawing;

namespace WinFormsApp2
{
    public class Player
    {
        public int posX;
        public int posY;
        public Image image;
        
        public Player(int posX, int posY, Image image)
        {
            this.posX = posX;
            this.posY = posY;
            this.image = image;
        }
    }
}