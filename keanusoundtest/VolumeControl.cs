using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;

namespace keanusoundtest
{
    class VolumeControl
    {
        private const int APPCOMMAND_VOLUME_UP = 0xA0000;
        private const int WM_APPCOMMAND = 0x319;
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessageW(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);
        public async void SubirVolumenAlMaximo(Window mainWindow)
        {
            await Task.Run(() =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    for (int i = 0; i < 100; i++)
                    {
                        SendMessageW(new WindowInteropHelper(mainWindow).Handle, WM_APPCOMMAND, new WindowInteropHelper(mainWindow).Handle, (IntPtr)APPCOMMAND_VOLUME_UP);
                    }
                });
            });
        }
    }
}
