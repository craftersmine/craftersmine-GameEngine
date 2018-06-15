using System;
using System.Runtime.Serialization;

namespace craftersmine.Packager.Lib.Core.Exceptions
{
    /// <summary>
    /// Throws if readed file haven't data to be written or null
    /// </summary>
    [Serializable]
    public class FileContentsNullException : Exception
    {
        /// <summary>
        /// Throws if readed file haven't data to be written or null
        /// </summary>
        public FileContentsNullException()
        {
        }
        /// <summary>
        /// Throws if readed file haven't data to be written or null
        /// </summary>
        /// <param name="message">Message to show</param>
        public FileContentsNullException(string message) : base(message)
        {
        }
        /// <summary>
        /// Throws if readed file haven't data to be written or null
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="innerException">Inner <see cref="Exception"/></param>
        public FileContentsNullException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Throws if readed file haven't data to be written or null
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected FileContentsNullException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}