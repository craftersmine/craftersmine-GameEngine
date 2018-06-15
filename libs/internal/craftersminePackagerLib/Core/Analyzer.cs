using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using craftersmine.Packager.Lib.Utils;
using craftersmine.Packager.Lib.Core.Exceptions;

namespace craftersmine.Packager.Lib.Core
{
    /// <summary>
    /// Represents an package contents analyzer. This class cannot be inherited
    /// </summary>
    public sealed class Analyzer
    {        
        /// <summary>
        /// Prehead of file
        /// </summary>
        private static byte prehd = 0x00;
        /// <summary>
        /// File header definition
        /// </summary>
        private static byte[] head = new byte[] { 0xc7, 0x56, 0x43, 0x4d, 0x50, 0x4b, 0x47, 0x00 };

        /// <summary>
        /// Analyzes package at sent path
        /// </summary>
        /// <param name="packagePath">Path of analyzable package file</param>
        /// <returns><see cref="PackageMetadata"/> of package file</returns>
        /// <exception cref="InvalidFiletypeException"></exception>
        /// <exception cref="InvalidVersionException"></exception>
        /// <exception cref="IOException"></exception>
        public static PackageMetadata AnalyzePackage(string packagePath)
        {
            using (BinaryReader reader = new BinaryReader(File.OpenRead(packagePath)))
            {
                byte prehead = reader.ReadByte();
                byte version = reader.ReadByte();
                byte[] header = reader.ReadBytes(8);
                if (prehead == prehd && ByteArrayValidator.ValidateBytes(head, header))
                {
                    switch (version)
                    {
                        case PackageVersions.Version1:
                            string packageNameString = reader.ReadString();
                            DateTime packageCreationTime = DateTime.FromBinary(reader.ReadInt64());
                            int filesTotal = reader.ReadInt32();
                            byte[] sep = new byte[] { 0x1f, 0x1f, 0xfd };
                            long _totalallBytes = 0;
                            List<PackageFileWithoutData> _files = new List<PackageFileWithoutData>();
                            for (int i = 0; i < filesTotal; i++)
                            {
                                string filename = reader.ReadString();
                                long filesize = reader.ReadInt64();
                                _totalallBytes += filesize;
                                _files.Add(new PackageFileWithoutData(filename, filesize));
                            }
                            PackageMetadata _meta = new PackageMetadata(packageNameString, _files.ToArray(), packageCreationTime, version);
                            return _meta;
                        case PackageVersions.Version2:
                            string packageNameString1 = reader.ReadString();
                            DateTime packageCreationTime1 = DateTime.FromBinary(reader.ReadInt64());
                            int filesTotal1 = reader.ReadInt32();
                            byte[] sep1 = new byte[] { 0x1f, 0x1f, 0xfd };
                            long _totalallBytes1 = 0;
                            List<PackageFileWithoutData> _files1 = new List<PackageFileWithoutData>();
                            for (int i = 0; i < filesTotal1; i++)
                            {
                                string filename = reader.ReadString();
                                long filesize = reader.ReadInt64();
                                _totalallBytes1 += filesize;
                                _files1.Add(new PackageFileWithoutData(filename, filesize));
                            }
                            PackageMetadata _meta1 = new PackageMetadata(packageNameString1, _files1.ToArray(), packageCreationTime1, version);
                            return _meta1;
                        default:
                            throw new InvalidVersionException(version);
                    }
                }
                else throw new InvalidFiletypeException();
            }
        }
    }

    /// <summary>
    /// Represents a package metadata
    /// </summary>
    public sealed class PackageMetadata
    {
        private PackageFileWithoutData[] _files;
        private string _pkgName;
        private DateTime _creationTime;
        private int _pkgrVer;
        private long _totalSize;

        /// <summary>
        /// Creates a new instance of <see cref="PackageMetadata"/>
        /// </summary>
        /// <param name="packageName">Internal written package name in file</param>
        /// <param name="files">Array of files in package without readed file contents</param>
        /// <param name="creationTime">Date and time, when package was created</param>
        /// <param name="packagerVersion">Minimal version of packager for this package</param>
        public PackageMetadata(string packageName, PackageFileWithoutData[] files, DateTime creationTime, int packagerVersion)
        {
            if (files != null)
                _files = files;
            else throw new ArgumentNullException("files");
            if (packageName != null)
                _pkgName = packageName;
            else throw new ArgumentNullException("packageName");
            _pkgrVer = packagerVersion;
            if (creationTime != null)
                _creationTime = creationTime;
            else throw new ArgumentNullException("creationTime");
            foreach (var fl in _files)
            {
                _totalSize += fl.Filesize;
            }
        }

        /// <summary>
        /// Array of files in package without file contents
        /// </summary>
        public PackageFileWithoutData[] PackageFiles { get { return _files; } }
        /// <summary>
        /// Internal written package name in file
        /// </summary>
        public string PackageName { get { return _pkgName; } }
        /// <summary>
        /// Date and time, when package was created
        /// </summary>
        public DateTime CreationTime { get { return _creationTime; } }
        /// <summary>
        /// Minimal version of packager for this package
        /// </summary>
        public int PackagerVersion { get { return _pkgrVer; } }
        /// <summary>
        /// Total size of all files in package
        /// </summary>
        public long TotalPackageFilesSize { get { return _totalSize; } }
    }

    /// <summary>
    /// Represents a minimal package file metadata
    /// </summary>
    public sealed class PackageFileWithoutData
    {
        private string _filename;
        private long _size;
        
        /// <summary>
        /// Creates a new instance of <see cref="PackageFileWithoutData"/>
        /// </summary>
        /// <param name="filename">File name</param>
        /// <param name="size">Size of file</param>
        public PackageFileWithoutData(string filename, long size)
        {
            _filename = filename;
            _size = size;
        }

        /// <summary>
        /// File name
        /// </summary>
        public string Filename { get { return _filename; } }
        /// <summary>
        /// Size of file
        /// </summary>
        public long Filesize { get { return _size; } }
    }
}
