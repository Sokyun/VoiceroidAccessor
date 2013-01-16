using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace net.azworks.VoiceroidAccess.voiceroids
{
    public class Yukari : Voiceroid
    {
        private IntPtr FindScroll(List<IntPtr> tgts)
        {
            foreach (var p in tgts)
                if (Win32.GetWindowClassName(p).Equals("ScrollBar"))
                    return p;
            return IntPtr.Zero;
        }
        
        private IntPtr FindTextBox()
        {
            List<IntPtr> targets = Win32.GetChildWindows(getVoiceroidWindwHandle());

            // 親子関係からScrollBarの親を検索する
            var scrollbar = FindScroll(targets);

            // scrollbar の兄弟が対象
            return Win32.GetWindow(scrollbar, Win32.GW_HWNDNEXT);
        }

        private IntPtr TalkButton()
        {
            var windowHandle = getVoiceroidWindwHandle();
            var granfa = Win32.GetWindow(windowHandle, Win32.GW_CHILD);
            var charrac = Win32.GetWindow(granfa, Win32.GW_CHILD);
            var buttongroup = Win32.GetWindow(charrac, Win32.GW_HWNDNEXT);
            var topbutton = Win32.GetWindow(buttongroup, Win32.GW_CHILD);
            IntPtr current = Win32.GetWindow(topbutton, Win32.GW_HWNDNEXT);
            current = Win32.GetWindow(current, Win32.GW_HWNDNEXT);
            current = Win32.GetWindow(current, Win32.GW_HWNDNEXT);
            return Win32.GetWindow(current, Win32.GW_HWNDNEXT);
        }

        IntPtr UNDO = new IntPtr(46);
        IntPtr PASTE = new IntPtr(56);
        IntPtr ALLSELECT = new IntPtr(60);
        IntPtr CUT = new IntPtr(52);

        public override void Talk(String message, int delay=100)
        {
            // 削除(全部選んでカット)
            Win32.SendMessage(getVoiceroidWindwHandle(), Win32.WM_COMMAND, ALLSELECT, IntPtr.Zero);
            Win32.SendMessage(getVoiceroidWindwHandle(), Win32.WM_COMMAND, CUT, IntPtr.Zero);
            System.Threading.Thread.Sleep(delay);

            using (this.GetActorMutex())
            {
                // コピペ
                Clipboard.SetText(message);
                System.Threading.Thread.Sleep(delay);
                Win32.SendMessage(getVoiceroidWindwHandle(), 273, PASTE, IntPtr.Zero);
                System.Threading.Thread.Sleep(delay);
            }
        }

        public override void Play()
        {
            Win32.SendMessage(TalkButton(), 0, IntPtr.Zero, IntPtr.Zero);
        }

        public override string VoiceroidName()
        {
            return "VOICEROID＋ 結月ゆかり";
        }

        public override String VoiceroidCode()
        {
            return "Yukari";
        }
    }
}
