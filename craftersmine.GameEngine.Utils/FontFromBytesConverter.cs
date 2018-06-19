using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Utils
{
    /// <summary>
    /// Gives static method of conversion <code>byte</code> array into <see cref="FontFamily"/>
    /// </summary>
    public sealed class FontFromBytesConverter
    {
        /// <summary>
        /// Converts <code>byte</code> array into <see cref="FontFamily"/>
        /// </summary>
        /// <param name="byteArrayIn">Input of <code>byte</code> array</param>
        /// <returns>Returns <see cref="FontFamily"/></returns>
        public static FontFamily FontFamilyFromBytes(byte[] byteArrayIn)
        {
            using (var pvc = new PrivateFontCollection())
            {
                var handlePtr = Marshal.AllocCoTaskMem(byteArrayIn.Length);
                Marshal.Copy(byteArrayIn, 0, handlePtr, byteArrayIn.Length);
                pvc.AddMemoryFont(handlePtr, byteArrayIn.Length);
                return pvc.Families[0];
            }
        }
    }
}
