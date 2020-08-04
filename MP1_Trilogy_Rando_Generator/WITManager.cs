using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MP1_Trilogy_Rando_Generator
{
    class WITManager
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        private const string WIT_PATH = @".\wit\bin\wit.exe";

        public static bool CreateCompressISO(string filename, bool isGC_ISO, String GameCode)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, "COPY -d \""+filename+"\" -s .\\tmp\\wii -C  --id " + GameCode);
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.UseShellExecute = true;
                Process proc = Process.Start(info);
                Thread.Sleep(1000);
                SetWindowText(proc.MainWindowHandle, "Creating Compressed "+(isGC_ISO ? "GC" : "Wii")+" ISO...");
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateWBFS(string filename, String GameCode)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, "COPY -s \".\\tmp\\wii\" -d \"" + filename + "\" -B --id \"" + GameCode + "\"");
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.UseShellExecute = true;
                Process proc = Process.Start(info);
                Thread.Sleep(1000);
                SetWindowText(proc.MainWindowHandle, "Creating WBFS...");
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }
    }
}
