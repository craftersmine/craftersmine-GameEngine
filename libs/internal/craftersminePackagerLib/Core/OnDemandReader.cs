using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using craftersmine.Packager.Lib.Core;
using System.IO;
using craftersmine.Packager.Lib.Core.Exceptions;
using craftersmine.Packager.Lib.Utils;

namespace craftersmine.Packager.Lib.Core
{
    public sealed class OnDemandPackage
    {
        private string _packagePath;
        private PackageMetadata _packageMetadata;
        private int _fileCount;
        private long _fileSize;
        private int _filePosition;

        public OnDemandPackage(string path)
        {
            if (path != string.Empty || path != null)
            {
                PackagePath = path;

                _packageMetadata = Analyzer.AnalyzePackage(PackagePath);
                
            }
            else throw new ArgumentNullException("path");
        }

        public byte[] ReadBytes(string filename)
        {
            List<byte[]> fileChunks = new List<byte[]>();
            List<byte> file = new List<byte>();
            try
            {
                using (BinaryReader reader = new BinaryReader(File.OpenRead(PackagePath)))
                {
                    byte prehead = reader.ReadByte();
                    byte version = reader.ReadByte();
                    byte[] header = reader.ReadBytes(8);
                    switch (version)
                    {
                        case PackageVersions.Version1:
                            string packageNameString = reader.ReadString();
                            DateTime packageCreationTime = DateTime.FromBinary(reader.ReadInt64());
                            int filesTotal = reader.ReadInt32();
                            byte[] sep = new byte[] { 0x1f, 0x1f, 0xfd };
                            long _toSeek = 3;
                            int fileId = 0;
                            for (int i = 0; i < filesTotal; i++)
                            {
                                string filename_ = reader.ReadString();
                                long filesize = reader.ReadInt64();
                            }
                            for (int i = 0; i < _packageMetadata.PackageFiles.Length; i++)
                            {
                                fileId = i;
                                if (filename != _packageMetadata.PackageFiles[i].Filename)
                                    _toSeek += _packageMetadata.PackageFiles[i].Filesize;
                                else break;
                            }
                            reader.BaseStream.Seek(_toSeek, SeekOrigin.Current);
                            if (_packageMetadata.PackageFiles[fileId].Filesize > int.MaxValue)
                            {
                                int chunkCount = (int)(_packageMetadata.PackageFiles[fileId].Filesize / int.MaxValue);
                                for (int j = 0; j < chunkCount; j++)
                                {
                                    byte[] buffer = reader.ReadBytes(int.MaxValue);
                                    fileChunks.Add(buffer);
                                }
                                foreach (var chunk in fileChunks)
                                {
                                    file.AddRange(chunk);
                                }
                            }
                            else file.AddRange(reader.ReadBytes((int)_packageMetadata.PackageFiles[fileId].Filesize));
                            break;
                        case PackageVersions.Version2:
                            string packageNameString1 = reader.ReadString();
                            DateTime packageCreationTime1 = DateTime.FromBinary(reader.ReadInt64());
                            int filesTotal1 = reader.ReadInt32();
                            byte[] sep1 = new byte[] { 0x1f, 0x1f, 0xfd };
                            long _toSeek1 = 3;
                            int fileId1 = 0;
                            for (int i = 0; i < filesTotal1; i++)
                            {
                                string filename_ = reader.ReadString();
                                long filesize = reader.ReadInt64();
                            }
                            for (int i = 0; i < _packageMetadata.PackageFiles.Length; i++)
                            {
                                fileId1 = i;
                                if (filename != _packageMetadata.PackageFiles[i].Filename)
                                    _toSeek1 += _packageMetadata.PackageFiles[i].Filesize;
                                else break;
                            }
                            reader.BaseStream.Seek(_toSeek1, SeekOrigin.Current);
                            if (_packageMetadata.PackageFiles[fileId1].Filesize > int.MaxValue)
                            {
                                int chunkCount = (int)(_packageMetadata.PackageFiles[fileId1].Filesize / int.MaxValue);
                                for (int j = 0; j < chunkCount; j++)
                                {
                                    byte[] buffer = reader.ReadBytes(int.MaxValue);
                                    fileChunks.Add(buffer);
                                }
                                foreach (var chunk in fileChunks)
                                {
                                    file.AddRange(chunk);
                                }
                            }
                            else file.AddRange(reader.ReadBytes((int)_packageMetadata.PackageFiles[fileId1].Filesize));
                            break;
                        default:
                            throw new InvalidVersionException(version);
                    }
                    return file.ToArray();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public string[] ReadLines(string filename)
        {
            string file = ReadText(filename);
            string[] lines = file.Split(new string[] { "\r\n", "\r", "\n" }, StringSplitOptions.None);
            return lines;
        }

        public string ReadText(string filename)
        {
            byte[] file = ReadBytes(filename);
            string fileStr = Encoding.Default.GetString(file);
            return fileStr;
        }

        public string PackagePath { get => _packagePath; private set => _packagePath = value; }
    }
}
