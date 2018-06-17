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
            var handle = GCHandle.Alloc(byteArrayIn, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(byteArrayIn, 0);
                using (var pvc = new PrivateFontCollection())
                {
                    pvc.AddMemoryFont(ptr, byteArrayIn.Length);
                    return pvc.Families[0];
                }
            }
            finally
            {
                handle.Free();
            }
        }
    }
}
