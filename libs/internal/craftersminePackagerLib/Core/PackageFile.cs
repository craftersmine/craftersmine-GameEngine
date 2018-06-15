using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace craftersmine.Packager.Lib.Core
{
    /// <summary>
    /// Represents a package file, ready to be written
    /// </summary>
    public sealed class PackageFile
    {
        /// <summary>
        /// Creates a new instance of <see cref="PackageFile"/>
        /// </summary>
        /// <param name="name">Name of package</param>
        /// <param name="pkgCreationTime">Overrides a auto-added creation time</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PackageFile(string name, DateTime pkgCreationTime)
        {
            if (pkgCreationTime != null)
                _pkgCreationTime = pkgCreationTime;
            else throw new ArgumentNullException("pkgCreationTime");
            if (name != null)
                _pkgname = name;
            else throw new ArgumentNullException("name");
        }

        /// <summary>
        /// Creates a new instance of <see cref="PackageFile"/>
        /// </summary>
        /// <param name="name">Name of package</param>
        /// <exception cref="ArgumentNullException"></exception>
        public PackageFile(string name) : this (name, DateTime.Now)
        {

        }

        private string _pkgname = "";
        private List<PackageEntry> _files = new List<PackageEntry>();
        private DateTime _pkgCreationTime = DateTime.Now;

        /// <summary>
        /// Name of package
        /// </summary>
        public string PackageName { get { return _pkgname; } }
        /// <summary>
        /// Array of <see cref="PackageEntry"/> with files ready to be written
        /// </summary>
        public PackageEntry[] Files { get { return _files.ToArray(); } }
        /// <summary>
        /// Package creation time
        /// </summary>
        public DateTime PackageCreationTime { get { return _pkgCreationTime; } }

        /// <summary>
        /// Adds a new file in <see cref="Files"/> array
        /// </summary>
        /// <param name="filepath">Full path of addable file</param>
        /// <exception cref="IOException"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PathTooLongException"></exception>
        /// <exception cref="DirectoryNotFoundException"></exception>
        /// <exception cref="UnauthorizedAccessException"></exception>
        /// <exception cref="FileNotFoundException"></exception>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="System.Security.SecurityException"></exception>
        public void AddFile(string filepath)
        {
            try
            {
                byte[] _cont = File.ReadAllBytes(filepath);
                PackageEntry _file = new PackageEntry(Path.GetFileNameWithoutExtension(filepath), Path.GetExtension(filepath), _cont);
                _files.Add(_file);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Removes a file from <see cref="Files"/> array
        /// </summary>
        /// <param name="filename">Name of removable file</param>
        /// <param name="extention">Extention of removable file</param>
        /// <returns></returns>
        public bool RemoveFile(string filename, string extention)
        {
            bool _isFoundAndRemoved = false;
            foreach (var entry in _files)
            {
                if (entry.Filename == filename && entry.Extention == extention)
                {
                    _files.Remove(entry);
                    if (!_files.Contains(entry))
                    {
                        _isFoundAndRemoved = true;
                    }
                    else
                    {
                        _isFoundAndRemoved = false;
                    }
                }
                else continue;
            }
            return _isFoundAndRemoved;
        }

        /// <summary>
        /// Clears all <see cref="Files"/> array
        /// </summary>
        public void ClearFilelist()
        {
            _files.Clear();
        }
    }
}
