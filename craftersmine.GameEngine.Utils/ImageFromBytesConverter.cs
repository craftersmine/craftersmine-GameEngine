using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace craftersmine.GameEngine.Utils
{
    /// <summary>
    /// Gives static method of conversion <code>byte</code> array into <see cref="Image"/>
    /// </summary>
    public sealed class ImageFromBytesConverter
    {
        /// <summary>
        /// Converts <code>byte</code> array into <see cref="Image"/>
        /// </summary>
        /// <param name="byteArrayIn">Input of <code>byte</code> array</param>
        /// <returns>Returns <see cref="Image"/></returns>
        public static Image ByteArrayToImage(byte[] byteArrayIn)
        {
            Image returnImage = null;
            if (byteArrayIn != null)
            {
                MemoryStream ms = new MemoryStream(byteArrayIn);
                ms.Position = 0;
                returnImage = Image.FromStream(ms);
            }
            return returnImage;
        }
    }
}
