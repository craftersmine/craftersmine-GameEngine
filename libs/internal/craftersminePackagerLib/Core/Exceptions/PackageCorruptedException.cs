using System;
using System.Runtime.Serialization;

namespace craftersmine.Packager.Lib.Core.Exceptions
{
    /// <summary>
    /// Throws if readed package corrupted
    /// </summary>
    [Serializable]
    public class PackageCorruptedException : Exception
    {
        /// <summary>
        /// Throws if readed package corrupted
        /// </summary>
        public PackageCorruptedException()
        {
        }
        /// <summary>
        /// Throws if readed package corrupted
        /// </summary>
        /// <param name="message">Message to show</param>
        public PackageCorruptedException(string message) : base(message)
        {
        }
        /// <summary>
        /// Throws if readed package corrupted
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="innerException">Inner <see cref="Exception"/></param>
        public PackageCorruptedException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Throws if readed package corrupted
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected PackageCorruptedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}