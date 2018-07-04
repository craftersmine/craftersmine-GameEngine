using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.System
{
    public sealed class RectangleObject
    {
        public Color BorderColor
        {
            get
            {
                if (BorderPen != null)
                    return BorderPen.Color;
                else return Color.Black;
            }
            set
            {
                if (BorderPen != null)
                    BorderPen.Color = value;
            }
        }
        public Color FillColor
        {
            get
            {
                if (FillBrush != null)
                    return FillBrush.Color;
                else return Color.Black;
            }
            set
            {
                if (FillBrush != null)
                    FillBrush.Color = value;
            }
        }
        
        internal Rectangle Rect { get; set; }
        internal Pen BorderPen { get; set; }
        internal SolidBrush FillBrush { get; set; }

        public RectangleObject(Rectangle rect, Color borderColor, Color fillColor, int borderWidth)
        {
            BorderColor = borderColor;
            FillColor = fillColor;
            BorderPen = new Pen(BorderColor);
            FillBrush = new SolidBrush(FillColor);
            Rect = rect;
        }

        public RectangleObject(Rectangle rect, Color borderColor, Color fillColor) : this(rect, borderColor, fillColor, 1) { }
    }
}
