using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;
using System.Windows.Interop;

namespace LightningDevelopment
{
    /// <summary>
    /// Responsible for binding a global hotkey
    /// </summary>
    public class HotkeyBinder
    {
        private readonly IntPtr windowHandle;
        //API Imports
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool RegisterHotKey(
            IntPtr hWnd, // handle to window    
            int id, // hot key identifier    
            KeyModifiers fsModifiers, // key-modifier options    
            Keys vk    // virtual-key code    
            );
        [DllImport("user32.dll", SetLastError = true)]
        public static extern bool UnregisterHotKey(
            IntPtr hWnd, // handle to window    
            int id      // hot key identifier    
            );
        const int HOTKEY_ID = 31197; //Any number to use to identify the hotkey instance
        public enum KeyModifiers        //enum to call 3rd parameter of RegisterHotKey easily
        {
            None = 0,
            Alt = 1,
            Control = 2,
            Shift = 4,
            Windows = 8
        }

        public HotkeyBinder(IntPtr windowHandle)
        {
            this.windowHandle = windowHandle;
        }


        public event Action HotkeyFired;

        public void WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_HOTKEY:
                    Keys key = (Keys)(((int)lParam >> 16) & 0xFFFF);
                    KeyModifiers modifier = (KeyModifiers)((int)lParam & 0xFFFF);
                    //put your on hotkey code here
                    //MessageBox.Show("HotKey Pressed :" + modifier.ToString() + " " + key.ToString());
                    //end hotkey code
                    if (HotkeyFired != null)
                        HotkeyFired();
                    handled = true;
                    break;
            }
        }

        public bool setHotKey(KeyModifiers Kmds, Keys key)
        {
            return RegisterHotKey(windowHandle, HOTKEY_ID, Kmds, key);
        }
        public bool unSetHotKey()
        {
            return UnregisterHotKey(windowHandle, HOTKEY_ID);
        }
        const int WM_HOTKEY = 0x0312;//magic hotkey message identifier
    }
}
