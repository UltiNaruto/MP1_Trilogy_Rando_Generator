using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace MP1_Trilogy_Rando_Generator
{
    class NodManager
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        private const string WIT_PATH = @"nod\nod.exe";

        public static bool ExtractISO(string filename, bool isGC_ISO)
        {
            if (!Directory.Exists(".\\tmp\\" + (isGC_ISO ? "gc" : "wii")))
                Directory.CreateDirectory(".\\tmp\\" + (isGC_ISO ? "gc" : "wii"));
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, "extract -f \"" + filename + "\" .\\tmp\\"+ (isGC_ISO?"gc":"wii"));
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.UseShellExecute = true;
                Process proc = Process.Start(info);
                Thread.Sleep(1000);
                SetWindowText(proc.MainWindowHandle, "Extracting "+(isGC_ISO ? "GC" : "Wii")+" ISO...");
                proc.WaitForExit();
                return proc.ExitCode == 0;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateISO(string filename, bool isGC_ISO)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, (isGC_ISO?"makegcn .\\tmp\\gc":"makewii .\\tmp\\wii") + " \"" + filename+"\"");
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.UseShellExecute = true;
                Process proc = Process.Start(info);
                Thread.Sleep(1000);
                SetWindowText(proc.MainWindowHandle, "Creating " + (isGC_ISO ? "GC" : "Wii") + " ISO...");
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
