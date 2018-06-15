using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.Packager.Lib.Core.Exceptions;

namespace craftersmine.Packager.Lib.Core
{
    /// <summary>
    /// Represents a package file entry
    /// </summary>
    public sealed class PackageEntry
    {
        /// <summary>
        /// Creates a new instance of <see cref="PackageEntry"/>
        /// </summary>
        /// <param name="filename">File name of entry</param>
        /// <param name="extention">Extention of entry</param>
        /// <param name="contents">Contents of readed file</param>
        public PackageEntry(string filename, string extention, byte[] contents)
        {
            _filename = filename;
            _ext = extention;
            Contents = contents;
        }

        private string _filename = "";
        private string _ext = "";
        private byte[] _cont = null;

        /// <summary>
        /// File name of entry
        /// </summary>
        public string Filename { get { return _filename; } }
        /// <summary>
        /// Extention of entry
        /// </summary>
        public string Extention { get { return _ext; } }
        /// <summary>
        /// Contents of readed file
        /// </summary>
        public byte[] Contents {
            get
            {
                if (_cont == null)
                {
                    throw new FileContentsNullException("File contents is null!");
                }
                else return _cont;
            }
            private set
            {
                if (value != null)
                {
                    _cont = value;
                }
                else throw new FileContentsNullException("File contents is null!");
            }
        }
    }
}
