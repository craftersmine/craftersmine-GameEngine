using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace craftersmine.Packager.Lib.Core
{
    /// <summary>
    /// Packager main class. This class cannot be inherited
    /// </summary>
    //[Obsolete]
    public sealed class Packager
    {
        /// <summary>
        /// Initialize packager class instance
        /// </summary>
        /// <param name="directory">Directory where output package will be stored</param>
        /// <param name="package">Package to build</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Packager(string directory, PackageFile package)
        {
            if (package != null)
                Package = package;
            else throw new ArgumentNullException("package");
            if (directory != null || directory != string.Empty)
                Directory = directory;
            else throw new ArgumentNullException("directory");
        }

        private PackageFile _package = null;
        private string _dir = "";

        /// <summary>
        /// Package file to build
        /// </summary>
        public PackageFile Package { get { return _package; } private set { _package = value; } }
        /// <summary>
        /// Directory where output package will be stored
        /// </summary>
        public string Directory { get { return _dir; } private set { _dir = value; } }

        /// <summary>
        /// Starts packaging
        /// </summary>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="EndOfStreamException"></exception>
        public void Pack(int chunkSize = 4096, string customExtention = "cmpkg")
        {
            PackingEventArgs _pea = new PackingEventArgs() { CurrentFileByte = 0, CurrentFilename = "", TotalAllBytes = 0, TotalFileByte = 0 };
            PackingEvent?.Invoke(this, _pea);
            PackingDoneEventArgs _pdea = new PackingDoneEventArgs() { IsSuccessful = false };
            string filepath = Path.Combine(Directory, Package.PackageName + "." + customExtention);
            try
            {
                if (!System.IO.Directory.Exists(Directory))
                    System.IO.Directory.CreateDirectory(Directory);
                // 0x02, 0x1d, 0x1e - Start of Contents
                // 0x1c, 0x1d - File Separator
                // 0x1f, 0x1f, 0xfd - Filename/Content Separator
                
                long _totalAllBytes = 0;
                foreach (var ent in Package.Files)
                {
                    _totalAllBytes += ent.Contents.LongLength;
                }
                _pea.TotalAllBytes = _totalAllBytes;
                PackingEvent?.Invoke(this, _pea);
                using (BinaryWriter writer = new BinaryWriter(File.Create(filepath)))
                {
                    writer.Write(new byte[] { 0x00, 0x02, 0xc7, 0x56, 0x43, 0x4d, 0x50, 0x4b, 0x47, 0x00 });
                    writer.Write(Package.PackageName);
                    //writer.Write(new byte[] { 0xcd, 0xb0 });
                    writer.Write(Package.PackageCreationTime.ToBinary());
                    writer.Write(Package.Files.Length);

                    for (int i = 0; i < Package.Files.Length; i++)
                    {
                        writer.Write(Package.Files[i].Filename + Package.Files[i].Extention);
                        writer.Write(Package.Files[i].Contents.LongLength);
                        
                    }

                    writer.Write(new byte[] { 0x1f, 0x1f, 0xfd });
                    long _totalBytesWithMeta = _totalAllBytes + writer.BaseStream.Position;
                    for (int j = 0; j < Package.Files.Length; j++)
                    {
                        int fileChunksCount = (int)Math.Ceiling((double)Package.Files[j].Contents.LongLength / (double)chunkSize);

                        _pea.CurrentFilename = Package.Files[j].Filename;
                        _pea.TotalFileByte = Package.Files[j].Contents.LongLength;
                        _pea.CurrentFileIndex = j;
                        writer.BaseStream.WriteAsync(Package.Files[j].Contents, 0, (int)Package.Files[j].Contents.LongLength);
                        _pea.PackingPercentage = (int)(((double)writer.BaseStream.Position / _totalBytesWithMeta) * 100.0d);
                        PackingEvent?.Invoke(this, _pea);
                    }
                }
                _pdea.IsSuccessful = true;
                PackingDoneEvent?.Invoke(this, _pdea);
            }
            catch (Exception e)
            {
                _pdea.InnerException = e;
                PackingDoneEvent?.Invoke(this, _pdea);
            }
        }

        /// <summary>
        /// Packing progress changed event delegate
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event data</param>
        public delegate void PackingEventDelegate(object sender, PackingEventArgs e);
        /// <summary>
        /// Calls if packing progress changed
        /// </summary>
        public event PackingEventDelegate PackingEvent;

        /// <summary>
        /// Packing complete event delegate
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event data</param>
        public delegate void PackingDoneEventDelegate(object sender, PackingDoneEventArgs e);
        /// <summary>
        /// Calls when packing completed
        /// </summary>
        public event PackingDoneEventDelegate PackingDoneEvent;
    }

    /// <summary>
    /// Data of packing completed event
    /// </summary>
    public class PackingDoneEventArgs : EventArgs
    {
        /// <summary>
        /// Exception of error if <see cref="IsSuccessful"/> is false. May be <code>null</code>
        /// </summary>
        public Exception InnerException { get; set; }
        /// <summary>
        /// Is packing successful
        /// </summary>
        public bool IsSuccessful { get; set; }
    }

    /// <summary>
    ///  Data of packing progress changed event
    /// </summary>
    public class PackingEventArgs : EventArgs
    {
        /// <summary>
        /// Current processing filename
        /// </summary>
        public string CurrentFilename { get; set; }
        /// <summary>
        /// Current processing byte of <see cref="CurrentFilename"/>
        /// </summary>
        [Obsolete]
        public long CurrentFileByte { get; set; }
        /// <summary>
        /// Total size in bytes of <see cref="CurrentFilename"/>
        /// </summary>
        [Obsolete]
        public long TotalFileByte { get; set; }
        /// <summary>
        /// Total size in bytes of all files
        /// </summary>
        [Obsolete]
        public long TotalAllBytes { get; set; }
        /// <summary>
        /// Current file index in array, starts from 0
        /// </summary>
        public int CurrentFileIndex { get; set; }

        public int PackingPercentage { get; set; }
    }
}
