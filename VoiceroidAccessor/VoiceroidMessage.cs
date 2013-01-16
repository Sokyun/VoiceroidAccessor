using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace net.azworks.VoiceroidAccess
{
    class VoiceroidMessage
    {
        public VoiceroidMessage(string message, int talkSec=0)
        {
            this.Message = message;
            this.Delay = talkSec;
        }

        public String Message { get; set; }
        public int Delay { get; private set; }
    }
}
