using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.System.UI
{
    public sealed class Label
    {
        internal SolidBrush LabelBrush { get; set; }
        internal RectangleF Bounds { get; set; }

        public string Text { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public Color LabelColor {
            get
            {
                if (LabelBrush != null)
                    return LabelBrush.Color;
                else return Color.Black;
            }
            set
            {
                if (LabelBrush != null)
                    LabelBrush.Color = value;
            }
        }
        public Font Font { get; set; }

        public Label(string text, int x, int y, int width, int height, Color color, Font font)
        {
            LabelBrush = new SolidBrush(color);
            LabelColor = color;
            X = x;
            Y = y;
            Width = width;
            Height = height;
            Text = text;
            Font = font;
            Bounds = new RectangleF(x, y, width, height);
        }

        public Label(string text, int x, int y, Color color, Font font) : this(text, x, y, 1000, 1000, color, font) { }
    }
}
