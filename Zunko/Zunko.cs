using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;

namespace net.azworks.VoiceroidAccess.voiceroids
{
    public class Zunko : Voiceroid
    {
        private IntPtr FindButton(List<IntPtr> tgts, String caption)
        {
            foreach (var p in tgts)
            {
                string className = Win32.GetWindowClassName(p);
                string captionText = Win32.GetWindowCaption(p);
                bool left = className.StartsWith("WindowsForms10.BUTTON");
                bool right = captionText.Contains(caption);
                if (left && right)
                    return p;
            }
            return IntPtr.Zero;
        }

        private IntPtr GetPlayButton()
        {
            var children = Win32.GetChildWindows(getVoiceroidWindwHandle());
            return FindButton(children, "再生");
        }

        private IntPtr GetTextbox()
        {
            IntPtr current = Win32.GetWindow(getVoiceroidWindwHandle(), Win32.GW_CHILD);
            current = Win32.GetWindow(current, Win32.GW_CHILD);
            current = Win32.GetWindow(current, Win32.GW_HWNDNEXT);
            current = Win32.GetWindow(current, Win32.GW_CHILD);
            current = Win32.GetWindow(current, Win32.GW_CHILD);
            current = Win32.GetWindow(current, Win32.GW_CHILD);
            current = Win32.GetWindow(current, Win32.GW_CHILD);
            return Win32.GetWindow(current, Win32.GW_CHILD);
        }

        IntPtr VK_DELETE = new IntPtr(46);

        public override void Talk(String message, int delay = 100)
        {
            IntPtr textEdit = GetTextbox();

            // 表示をすべて削除
            IntPtr currentTextLength = Win32.SendMessage(textEdit, Win32.WM_GETTEXTLENGTH, IntPtr.Zero, IntPtr.Zero);
            Win32.SendMessage(textEdit, Win32.EM_SETSEL, IntPtr.Zero, new IntPtr(currentTextLength.ToInt32()));
            Win32.SendMessage(textEdit, Win32.WM_KEYDOWN, new IntPtr(46), IntPtr.Zero);
            System.Threading.Thread.Sleep(delay);

            using (this.GetActorMutex())
            {
                Clipboard.SetText(message);
                System.Threading.Thread.Sleep(delay);
                Win32.SendMessage(textEdit, Win32.WM_PASTE, IntPtr.Zero, IntPtr.Zero);
                System.Threading.Thread.Sleep(delay);
            }
        }

        public override void Play()
        {
            IntPtr playButton = GetPlayButton();
            Win32.SendMessage(playButton, 245, IntPtr.Zero, IntPtr.Zero);
        }

        public override string VoiceroidName()
        {
            return "VOICEROID＋ 東北ずん子";
        }

        public override String VoiceroidCode()
        {
            return "Zunko";
        }
    }
}
