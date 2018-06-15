using System;
using System.Runtime.Serialization;

namespace craftersmine.Packager.Lib.Core.Exceptions
{
    /// <summary>
    /// Throws if readed file have invalid type or corrupted head
    /// </summary>
    [Serializable]
    public class InvalidFiletypeException : Exception
    {
        /// <summary>
        /// Throws if readed file have invalid type or corrupted head
        /// </summary>
        public InvalidFiletypeException()
        {
        }
        /// <summary>
        /// Throws if readed file have invalid type or corrupted head
        /// </summary>
        /// <param name="message">Message to show</param>
        public InvalidFiletypeException(string message) : base(message)
        {
        }
        /// <summary>
        /// Throws if readed file have invalid type or corrupted head
        /// </summary>
        /// <param name="message">Message to show</param>
        /// <param name="innerException">Inner <see cref="Exception"/></param>
        public InvalidFiletypeException(string message, Exception innerException) : base(message, innerException)
        {
        }
        /// <summary>
        /// Throws if readed file have invalid type or corrupted head
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected InvalidFiletypeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}