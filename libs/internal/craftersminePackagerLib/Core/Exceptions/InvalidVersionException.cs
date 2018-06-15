using System;
using System.Runtime.Serialization;

namespace craftersmine.Packager.Lib.Core.Exceptions
{
    /// <summary>
    /// Throws if package have invalid or unsupported packager verion
    /// </summary>
    [Serializable]
    public class InvalidVersionException : Exception
    {
        private int version;
        /// <summary>
        /// Version of extracting or analyzing package
        /// </summary>
        public int PackageVersion { get { return version; } }
        /// <summary>
        /// Throws if package have invalid or unsupported packager verion
        /// </summary>
        public InvalidVersionException()
        {
        }
        /// <summary>
        /// Throws if package have invalid or unsupported packager verion
        /// </summary>
        /// <param name="version">Version of package</param>
        public InvalidVersionException(int version)
        {
            this.version = version;
        }
        /// <summary>
        /// Throws if package have invalid or unsupported packager verion
        /// </summary>
        /// <param name="message">Message to show</param>
        public InvalidVersionException(string message) : base(message)
        {
        }
        /// <summary>
        /// Throws if package have invalid or unsupported packager verion
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="innerException">Inner <see cref="Exception"/></param>
        public InvalidVersionException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Throws if package have invalid or unsupported packager verion
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidVersionException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}