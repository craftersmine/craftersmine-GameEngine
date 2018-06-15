using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.Packager.Lib.Utils
{
    internal class ByteArrayValidator
    {
        internal static bool ValidateBytes(byte[] bytearr1, byte[] bytearr2)
        {
            bool _isValid = false;
            if (bytearr2.Length == bytearr1.Length)
            {
                for (int i = 0; i < bytearr1.Length; i++)
                {
                    if (bytearr2[i] == bytearr1[i])
                    {
                        _isValid = true;
                    }
                    else _isValid = false;
                }
            }
            return _isValid;
        }
    }
}
