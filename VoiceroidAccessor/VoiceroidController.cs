using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using net.azworks.VoiceroidAccess.voiceroids;

namespace net.azworks.VoiceroidAccess
{
    public class VoiceroidController
    {
        private Voiceroid selectedVoiceroid;
        private VoiceSetting EnvDt = new VoiceSetting();
        private VoiceroidTalk talk = null;

        public VoiceSetting Settings
        {
            get { return this.EnvDt; }
            set { this.EnvDt = value; }
        }

        public String GetVoiceroidName()
        {
            return this.selectedVoiceroid.VoiceroidName();
        }

        public VoiceroidController(Voiceroid voiceroid)
        {
            this.selectedVoiceroid = voiceroid;
            this.talk = new VoiceroidTalk(this.selectedVoiceroid, this.EnvDt);
        }

        public void Talk(String talking, long talkSec = 0)
        {
            talk.SendMessage(new VoiceroidMessage(talking));
        }

        private String ConfFileName()
        {
            return String.Format(@"conf/{0}.xml", this.selectedVoiceroid.VoiceroidCode());
        }

        public void SaveConf()
        {
            System.Xml.Serialization.XmlSerializer serializer1
                = new System.Xml.Serialization.XmlSerializer(typeof(VoiceSetting));

            using (System.IO.FileStream fs1 = new System.IO.FileStream(ConfFileName(), System.IO.FileMode.Create))
            {
                serializer1.Serialize(fs1, this.Settings);
            }
        }

        public void LoadConf()
        {
            System.Xml.Serialization.XmlSerializer serializer1
                = new System.Xml.Serialization.XmlSerializer(typeof(VoiceSetting));
            try
            {
                using (System.IO.FileStream fs2 = new System.IO.FileStream(ConfFileName(), System.IO.FileMode.Open))
                {
                    this.Settings = (VoiceSetting)serializer1.Deserialize(fs2);
                }
            }
            catch (Exception e)
            {
                this.SaveConf();
            }
        }
    }

    public class VoiceSetting
    {
        public VoiceSetting()
        {
            DelayPerWord = 250;
            DelayPerLine = 300;
        }

        public int DelayPerWord { get; set; }
        public int DelayPerLine { get; set; }
    }
}
