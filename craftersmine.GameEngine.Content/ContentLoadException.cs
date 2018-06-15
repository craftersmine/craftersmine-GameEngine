using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace craftersmine.GameEngine.Content
{
    [System.Serializable]
    public class ContentLoadException : Exception
    {
        public ContentLoadException() { }
        public ContentLoadException(string message) : base(message) { }
        public ContentLoadException(string message, Exception inner) : base(message, inner) { }
        protected ContentLoadException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
