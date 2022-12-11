using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApp1
{
    class GameSprite
    {
        public Bitmap SpriteImage { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int Color { get; set; }
        public bool Matches { get; set; }

        public GameSprite()
        {
            Color = 0;
            Matches = false;
        }

        public void Draw(Graphics gfx)
        {
            // Draw sprite image on screen
            gfx.DrawImage(SpriteImage, new RectangleF(X, Y, Width, Height));
        }
    }
}
