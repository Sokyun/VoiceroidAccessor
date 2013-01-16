using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace net.azworks.VoiceroidAccess.messages
{
    public class MessageChanger
    {
        private List<ChangeDefinition> changes = new List<ChangeDefinition>();

        public List<ChangeDefinition> Changes
        {
            get { return this.changes; }
            set { this.changes = value; }
        }

        public string Change(string target)
        {
            foreach (var chenge in this.Changes)
            {
                if (chenge.From.IsMatch(target))
                    target = chenge.From.Replace(target, chenge.To);
            }

            return target;
        }

        public void LoadLists()
        {
            try
            {
                using (StreamReader reader = new StreamReader(@"conf/changelist.txt"))
                {
                    while (true)
                    {
                        string fromline = reader.ReadLine();
                        string toline = reader.ReadLine();
                        if (fromline == null || toline == null)
                            break;
                        this.changes.Add(new ChangeDefinition(fromline, toline));
                    }
                }
            }
            catch (Exception e)
            {
                return;
            }
        }

        public void SaveLists()
        {
            using (StreamWriter writer = new StreamWriter(@"conf/changelist.txt", false))
            {
                foreach (var definition in Changes)
                {
                    writer.WriteLine(definition.From);
                    writer.WriteLine(definition.To);
                }
            }
        }
    }

    public class ChangeDefinition
    {
        private Regex regex = null;

        public Regex From
        {
            get { return this.regex; }
            set { this.regex = value; }
        }

        public string To { get; set; }

        public ChangeDefinition(string from, string to)
        {
            this.regex = new Regex(from);
            this.To = to;
        }
    }
}
