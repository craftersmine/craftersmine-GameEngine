using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using craftersmine.Packager.Lib.Core.Exceptions;
using craftersmine.Packager.Lib.Utils;

namespace craftersmine.Packager.Lib.Core
{
    /// <summary>
    /// Extractor main class. This class cannot be inherited
    /// </summary>
    public sealed class Extractor
    {
        /// <summary>
        /// Initialize extractor class instance
        /// </summary>
        /// <param name="directoryForExtracted">Directory where output files from package will be stored</param>
        /// <param name="packagePath">Package to extract</param>
        /// <exception cref="ArgumentNullException"></exception>
        public Extractor(string packagePath, string directoryForExtracted)
        {
            if (packagePath != string.Empty || packagePath != null)
                PackagePath = packagePath;
            else throw new ArgumentNullException("packagePath");
            if (directoryForExtracted != string.Empty || directoryForExtracted != null)
                DirectoryForExtracted = directoryForExtracted;
            else throw new ArgumentNullException("directoryForExtrated");
        }

        private string _packPath = "";
        private string _extractDir = "";

        /// <summary>
        /// Package full path for extract
        /// </summary>
        public string PackagePath { get { return _packPath; } set { _packPath = value; } }
        /// <summary>
        /// Directory where output files from package will be stored
        /// </summary>
        public string DirectoryForExtracted { get { return _extractDir; } set { _extractDir = value; } }

        /// <summary>
        /// Prehead of file
        /// </summary>
        private byte prehd = 0x00;
        /// <summary>
        /// File header definition
        /// </summary>
        private byte[] head = new byte[] { 0xc7, 0x56, 0x43, 0x4d, 0x50, 0x4b, 0x47, 0x00 };

        /// <summary>
        /// Extracting progress changed event delegate
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event data</param>
        public delegate void ExtractingEventDelegate(object sender, ExtractingEventArgs e);
        /// <summary>
        /// Calls if extraction progress changed
        /// </summary>
        public event ExtractingEventDelegate ExtractingEvent;

        /// <summary>
        /// Extracting complete event delegate
        /// </summary>
        /// <param name="sender">Object</param>
        /// <param name="e">Event data</param>
        public delegate void ExtractingDoneEventDelegate(object sender, ExtractingDoneEventArgs e);
        /// <summary>
        /// Calls when extraction completed
        /// </summary>
        public event ExtractingDoneEventDelegate ExtractingDoneEvent;

        /// <summary>
        /// Starts extraction
        /// </summary>
        /// <exception cref="InvalidFiletypeException"></exception>
        /// <exception cref="InvalidVersionException"></exception>
        /// <exception cref="PackageCorruptedException"></exception>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ObjectDisposedException"></exception>
        /// <exception cref="EndOfStreamException"></exception>
        public void Extract()
        {
            ExtractingEventArgs _eea = new ExtractingEventArgs();
            ExtractingDoneEventArgs _edea = new ExtractingDoneEventArgs()
            {
                IsSuccessful = false
            };
            try
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(PackagePath)))
                {
                    _eea.CurrentFilename = "VALIDATION";
                    _eea.CurrentFileByte = 0;
                    _eea.TotalAllBytes = 0;
                    _eea.TotalFileByte = 0;
                    ExtractingEvent?.Invoke(this, _eea);
                    byte prehead = reader.ReadByte();
                    byte version = reader.ReadByte();
                    byte[] header = reader.ReadBytes(8);
                    byte[] sep = new byte[] { 0x1f, 0x1f, 0xfd };
                    if (prehead == prehd && ByteArrayValidator.ValidateBytes(head, header))
                    {
                        switch (version)
                        {
                            case PackageVersions.Version1:
                                string packageNameString = reader.ReadString();
                                DateTime packageCreationTime = DateTime.FromBinary(reader.ReadInt64());
                                int filesTotal = reader.ReadInt32();
                                Dictionary<string, long> _filetable = new Dictionary<string, long>();
                                long _totalallBytes = 0;
                                for (int i = 0; i < filesTotal; i++)
                                {
                                    string filename = reader.ReadString();
                                    long filesize = reader.ReadInt64();
                                    _totalallBytes += filesize;
                                    _filetable.Add(filename, filesize);
                                }
                                _eea.TotalAllBytes = _totalallBytes;
                                ExtractingEvent?.Invoke(this, _eea);
                                if (!Directory.Exists(DirectoryForExtracted))
                                    Directory.CreateDirectory(DirectoryForExtracted);
                                if (ByteArrayValidator.ValidateBytes(reader.ReadBytes(3), sep))
                                {
                                    int currflctr = 0;
                                    foreach (var fileInTable in _filetable)
                                    {
                                        currflctr++;
                                        _eea.CurrentFilename = fileInTable.Key;
                                        _eea.TotalFileByte = fileInTable.Value;
                                        _eea.CurrentFileIndex = currflctr;
                                        ExtractingEvent?.Invoke(this, _eea);
                                        using (BinaryWriter writer = new BinaryWriter(File.Create(Path.Combine(DirectoryForExtracted, fileInTable.Key))))
                                        {
                                            for (long curByte = 0; curByte < fileInTable.Value; curByte++)
                                            {
                                                _eea.CurrentFileByte = curByte;
                                                ExtractingEvent?.Invoke(this, _eea);
                                                writer.Write(reader.ReadByte());
                                            }
                                        }
                                    }
                                }
                                else throw new PackageCorruptedException();
                                _edea.IsSuccessful = true;
                                ExtractingDoneEvent?.Invoke(this, _edea);
                                break;
                            case PackageVersions.Version2:
                                _eea.ExtractingPercentage = 0;
                                long _totalBytesWithMeta = 0;
                                string packageNameString2 = reader.ReadString();
                                DateTime packageCreationTime2 = DateTime.FromBinary(reader.ReadInt64());
                                int filesTotal2 = reader.ReadInt32();
                                Dictionary<string, long> _filetable2 = new Dictionary<string, long>();
                                long _totalallBytes2 = 0;
                                for (int i = 0; i < filesTotal2; i++)
                                {
                                    string filename = reader.ReadString();
                                    long filesize = reader.ReadInt64();
                                    _totalallBytes2 += filesize;
                                    _filetable2.Add(filename, filesize);
                                }
                                _totalBytesWithMeta = reader.BaseStream.Position + _totalallBytes2;
                                _eea.TotalAllBytes = _totalallBytes2;
                                ExtractingEvent?.Invoke(this, _eea);
                                if (!Directory.Exists(DirectoryForExtracted))
                                    Directory.CreateDirectory(DirectoryForExtracted);
                                if (ByteArrayValidator.ValidateBytes(reader.ReadBytes(3), sep))
                                {
                                    int currflctr = 0;
                                    foreach (var fileInTable in _filetable2)
                                    {
                                        currflctr++;
                                        _eea.CurrentFilename = fileInTable.Key;
                                        _eea.TotalFileByte = fileInTable.Value;
                                        _eea.CurrentFileIndex = currflctr;
                                        ExtractingEvent?.Invoke(this, _eea);
                                        using (BinaryWriter writer = new BinaryWriter(File.Create(Path.Combine(DirectoryForExtracted, fileInTable.Key))))
                                        {
                                            _eea.CurrentFilename = fileInTable.Key;
                                            _eea.TotalFileByte = fileInTable.Value;
                                            _eea.CurrentFileIndex = currflctr;
                                            writer.BaseStream.Write(reader.ReadBytes((int)fileInTable.Value), 0, (int)fileInTable.Value);
                                            _eea.ExtractingPercentage = (int)(((double)reader.BaseStream.Position / _totalBytesWithMeta) * 100.0d);
                                            ExtractingEvent?.Invoke(this, _eea);
                                        }
                                    }
                                }
                                else throw new PackageCorruptedException();
                                _edea.IsSuccessful = true;
                                ExtractingDoneEvent?.Invoke(this, _edea);
                                break;
                            default:
                                throw new InvalidVersionException(version);
                        }
                    }
                    else throw new InvalidFiletypeException();
                }
            }
            catch (Exception e)
            {
                _edea.InnerException = e;
                ExtractingDoneEvent?.Invoke(this, _edea);
                throw e;
            }
        }
    }

    /// <summary>
    /// Data of extraction completed event
    /// </summary>
    public class ExtractingDoneEventArgs : EventArgs
    {
        /// <summary>
        /// Is extraction successful
        /// </summary>
        public bool IsSuccessful { get; set; }
        /// <summary>
        /// Exception of error if <see cref="IsSuccessful"/> is false. May be <code>null</code>
        /// </summary>
        public Exception InnerException { get; set; }
    }

    /// <summary>
    ///  Data of packing progress changed event
    /// </summary>
    public class ExtractingEventArgs : EventArgs
    {
        /// <summary>
        /// Current processing filename
        /// </summary>
        public string CurrentFilename { get; set; }
        /// <summary>
        /// Current processing byte of <see cref="CurrentFilename"/>
        /// </summary>
        public long CurrentFileByte { get; set; }
        /// <summary>
        /// Total size in bytes of <see cref="CurrentFilename"/>
        /// </summary>
        public long TotalFileByte { get; set; }
        /// <summary>
        /// Total size in bytes of all files
        /// </summary>
        public long TotalAllBytes { get; set; }
        /// <summary>
        /// Current file index in array, starts from 0
        /// </summary>
        public int CurrentFileIndex { get; set; }

        public int ExtractingPercentage { get; set; }
    }
}