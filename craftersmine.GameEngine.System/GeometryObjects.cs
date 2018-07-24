using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.System
{
    /// <summary>
    /// Represents a rectangle object. This class cannot be inherited
    /// </summary>
    public sealed class RectangleObject
    {
        /// <summary>
        /// Rectangle border color
        /// </summary>
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

        /// <summary>
        /// Rectangle fill color
        /// </summary>
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

        /// <summary>
        /// Creates new instance of <see cref="RectangleObject"/> with specified colors and border width
        /// </summary>
        /// <param name="rect">Rectangle boundings</param>
        /// <param name="borderColor">Rectangle border color</param>
        /// <param name="fillColor">Rectangle fill color</param>
        /// <param name="borderWidth">Rectangle border width</param>
        public RectangleObject(Rectangle rect, Color borderColor, Color fillColor, int borderWidth)
        {
            BorderColor = borderColor;
            FillColor = fillColor;
            BorderPen = new Pen(BorderColor);
            FillBrush = new SolidBrush(FillColor);
            Rect = rect;
        }
        /// <summary>
        /// Creates new instance of <see cref="RectangleObject"/> with specified colors and which border width is 1px
        /// </summary>
        /// <param name="rect">Rectangle boundings</param>
        /// <param name="borderColor">Rectangle border color</param>
        /// <param name="fillColor">Rectangle fill color</param>
        public RectangleObject(Rectangle rect, Color borderColor, Color fillColor) : this(rect, borderColor, fillColor, 1) { }
    }
}
