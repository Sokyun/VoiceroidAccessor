using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using net.azworks.threading;

namespace net.azworks.VoiceroidAccess.voiceroids
{
    public abstract class Voiceroid
    {
        abstract public String VoiceroidName();

        abstract public String VoiceroidCode();

        abstract public void Talk(String message, int delay = 10);
        
        abstract public void Play();

        protected UsingMutex GetActorMutex()
        {
            return new UsingMutex("net.azworks.VoiceroidAccess.voiceroids.Voiceroid");
        }
        
        protected Process getVoiceroidProcess()
        {
            Process[] proceses = Process.GetProcessesByName("VOICEROID");

            if (proceses.Length == 0)
                throw new ApplicationInirializeException(
                    "VOCALOID.exe プロセスが存在していません。");

            foreach (Process process in proceses)
            {
                string processName = process.ProcessName;
                string windowTitle = process.MainWindowTitle;
                if (process.MainWindowTitle.StartsWith(VoiceroidName()))
                    return process;
            }
            return null;
        }

        private Process process = null;

        protected IntPtr getVoiceroidWindwHandle()
        {
            try
            {
                if (process.HasExited)
                    process = null;
                return process.MainWindowHandle;
            }
            catch (Exception e)
            {
                process = getVoiceroidProcess();
                if (process == null)
                    throw new ApplicationInirializeException(
                        VoiceroidName() + " が起動してないっぽいです。");
                return process.MainWindowHandle;
            }
        }

        protected virtual void ResetHandles()
        {
            process = getVoiceroidProcess();
        }
    }
}
