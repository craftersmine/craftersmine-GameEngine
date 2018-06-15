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
    public sealed class FontFromBytesConverter
    {
        public static FontFamily FontFamilyFromBytes(byte[] buffer)
        {
            var handle = GCHandle.Alloc(buffer, GCHandleType.Pinned);
            try
            {
                var ptr = Marshal.UnsafeAddrOfPinnedArrayElement(buffer, 0);
                using (var pvc = new PrivateFontCollection())
                {
                    pvc.AddMemoryFont(ptr, buffer.Length);
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
