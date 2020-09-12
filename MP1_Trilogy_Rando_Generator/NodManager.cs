﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace MP1_Trilogy_Rando_Generator
{
    class NodManager
    {
        [DllImport("user32.dll")]
        static extern bool SetWindowText(IntPtr hWnd, string text);

        private const string NOD_PATH = @"nod\nod.exe";

        public static bool ExtractISO(string filename, bool isGC_ISO)
        {
            if (!Directory.Exists(".\\tmp\\" + (isGC_ISO ? "gc" : "wii")))
                Directory.CreateDirectory(".\\tmp\\" + (isGC_ISO ? "gc" : "wii"));
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(NOD_PATH, "extract -f \"" + filename + "\" .\\tmp\\"+ (isGC_ISO?"gc":"wii"));
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
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

        static bool SetTitleID(String title_id, bool isGC_ISO)
        {
            var path = isGC_ISO ? "gc" : "wii\\DATA";
            try
            {
                if (title_id.Length > 6)
                    return false;
                if (!isGC_ISO)
                {
                    using (var file = File.OpenWrite(".\\tmp\\" + path + "\\disc\\header.bin"))
                    using (var writer = new BinaryWriter(file))
                    {
                        writer.Write(Encoding.ASCII.GetBytes(title_id.ToCharArray(), 0, 6));
                    }
                }
                using (var file = File.OpenWrite(".\\tmp\\" + path + "\\sys\\boot.bin"))
                using (var writer = new BinaryWriter(file))
                {
                    writer.Write(Encoding.ASCII.GetBytes(title_id.ToCharArray(), 0, 6));
                    if(title_id.StartsWith("R3M"))
                        writer.Write((short)0);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CreateISO(string filename, bool isGC_ISO, String title_id)
        {
            try
            {
                if (!SetTitleID(title_id, isGC_ISO))
                    return false;
                
                ProcessStartInfo info = new ProcessStartInfo(NOD_PATH, (isGC_ISO?"makegcn .\\tmp\\gc":"makewii .\\tmp\\wii") + " \"" + filename+"\"");
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.CreateNoWindow = true;
                info.UseShellExecute = false;
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
