using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using net.azworks.VoiceroidAccess.voiceroids;
using net.azworks.VoiceroidAccess.messages;
using System.IO;
using System.Reflection;

namespace net.azworks.VoiceroidAccess
{
    public class VoiceroidManager
    {
        private Dictionary<String, VoiceroidController> manager = new Dictionary<string, VoiceroidController>();
        private MessageChanger changesets = new MessageChanger();

        public VoiceroidManager()
        {
            var controllers = LoadVoiceroids().Select((Voiceroid voiceroid) => new VoiceroidController(voiceroid));
            foreach (var tgt in controllers)
            {
                manager.Add(tgt.GetVoiceroidName(), tgt);
            }

            foreach (var pair in manager)
            {
                pair.Value.LoadConf();
            }

            changesets.LoadLists();
        }

        public void SaveConfigs()
        {
            foreach (var pair in manager)
            {
                pair.Value.SaveConf();
            }

            changesets.SaveLists();
        }

        public MessageChanger Chenger
        {
            get { return this.changesets; }
            set { this.changesets = value; }
        }

        public Dictionary<String, String> ChangeSets
        {
            get
            {
                var results = new Dictionary<String, String>();
                foreach (var definition in this.changesets.Changes)
                {
                    results[definition.From.ToString()] = definition.To;
                }
                return results;
            }

            set
            {
                List<ChangeDefinition> defs = new List<ChangeDefinition>();
                foreach (var pair in value)
                {
                    defs.Add(new ChangeDefinition(pair.Key, pair.Value));
                }
                this.changesets.Changes = defs;
            }
        }

        public Dictionary<String, VoiceroidController> Managers
        {
            get { return this.manager; }
            set { this.manager = value; }
        }

        public String[] allVoiceroid()
        {
            List<String> values = new List<string>();
            foreach (var pair in this.manager)
            {
                values.Add(pair.Key);
            }
            return values.ToArray();
        }

        public void Talk(String voicaroid, String talk, long talkSec=0)
        {
            VoiceroidController controller = manager[voicaroid];
            talk = changesets.Change(talk);
            talk = NGWords.CreateSafdWord(talk);
            controller.Talk(talk, talkSec);
        }

        private List<Voiceroid> LoadVoiceroids()
        {
            List<Voiceroid> list = new List<Voiceroid>();

            // iterate plugins
            string sPluginFolder = Environment.CurrentDirectory + "\\voiceroids";
            string[] aryDLLFilePath = Directory.GetFiles(sPluginFolder, "*.dll");

            if (aryDLLFilePath == null)
                return list;

            foreach (var dllPath in aryDLLFilePath)
            {
                // load assembly
                Assembly assembly = Assembly.LoadFile(dllPath);
                if (assembly == null)
                    continue;

                // get all types in assembly
                Type[] types = assembly.GetTypes();

                foreach (var type in types)
                {
                    // skip notClass, abstract, notPublic, isNotVisible
                    if (!type.IsClass || type.IsAbstract || type.IsNotPublic || !type.IsVisible)
                        continue;

                    // is type of Voiceroid
                    if (type.IsSubclassOf(typeof(Voiceroid)))
                    {
                        // get default constructor
                        ConstructorInfo ci = type.GetConstructor(Type.EmptyTypes);
                        if (ci == null)
                            continue;

                        // create instance
                        object instance = ci.Invoke(new object[] { });
                        if (instance == null)
                            continue;

                        Voiceroid pluginInstance = (Voiceroid)instance;

                        list.Add(pluginInstance);
                    }
                }
            }

            return list;
        }
    }
}
