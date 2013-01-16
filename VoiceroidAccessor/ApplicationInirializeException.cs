using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.azworks.VoiceroidAccess.voiceroids
{
    class ApplicationInirializeException : Exception
    {
        public ApplicationInirializeException(String message)
            : base(message)
        { }
    }
}
