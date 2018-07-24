using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.System.UI
{
    /// <summary>
    /// Represents a label. This class cannot be inherited
    /// </summary>
    public sealed class Label
    {
        internal SolidBrush LabelBrush { get; set; }
        internal RectangleF Bounds { get; set; }

        /// <summary>
        /// Gets or sets text string of label
        /// </summary>
        public string Text { get; set; }
        /// <summary>
        /// Gets or sets width of label
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        /// Gets or sets height of label
        /// </summary>
        public int Height { get; set; }
        /// <summary>
        /// Gets or sets position of label by X axis
        /// </summary>
        public int X { get; set; }
        /// <summary>
        /// Gets or sets position of label by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        /// Gets or sets label color
        /// </summary>
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
        /// <summary>
        /// Gets or sets font of label
        /// </summary>
        public Font Font { get; set; }

        /// <summary>
        /// Creates new instance of <see cref="Label"/> with specified string of text, position, size, color and font
        /// </summary>
        /// <param name="text">Label text string</param>
        /// <param name="x">Position of label by X axis</param>
        /// <param name="y">Position of label by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract</param>
        /// <param name="width">Width of label</param>
        /// <param name="height">Height of label</param>
        /// <param name="color">Label color</param>
        /// <param name="font">Font of label</param>
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
        /// <summary>
        /// Creates new instance of <see cref="Label"/> with specified string of text, position, color and font
        /// </summary>
        /// <param name="text">Label text string</param>
        /// <param name="x">Position of label by X axis</param>
        /// <param name="y">Position of label by Y axis. REMARK: Y axis is inverted! To move object down, you need to add value instead subtract</param>
        /// <param name="color">Label color</param>
        /// <param name="font">Font of label</param>
        public Label(string text, int x, int y, Color color, Font font) : this(text, x, y, 1000, 1000, color, font) { }
    }
}
