using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.azworks.VoiceroidAccess.voiceroids;
using System.Collections;
using System.Threading.Tasks;

namespace net.azworks.VoiceroidAccess
{
    class VoiceroidTalk : threading.ActorBase<VoiceroidMessage>
    {
        private Voiceroid voiceroid;
        private VoiceSetting talkSetting;

        public VoiceroidTalk(Voiceroid voiceroid, VoiceSetting setting)
        {
            this.voiceroid = voiceroid;
            talkSetting = setting;
        }

        protected override void Act(VoiceroidMessage message)
        {
            int defaultDelay = message.Message.Length * talkSetting.DelayPerWord + talkSetting.DelayPerWord;
            this.voiceroid.Talk(message.Message);
            this.voiceroid.Play();
            System.Threading.Thread.Sleep((message.Delay != 0) ? message.Delay : defaultDelay);
        }
    }
}
