using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace MP1_Trilogy_Rando_Generator
{
    class NodManager
    {

        private const string NOD_PATH = @"nod\nod.exe";
        private const string NOD_EXE_URL = "https://github.com/AxioDL/nod/releases/download/v1.0/nodtool.v1.win64.exe";
        private const string NOD_LICENSE_URL = "https://raw.githubusercontent.com/AxioDL/nod/master/LICENSE";

        public static bool Installed()
        {
            return File.Exists(NOD_PATH);
        }

        public static bool Init()
        {
            try
            {
                if (Installed())
                    return true;
                using (var client = new WebClientPlus())
                {
                    if (!Directory.Exists(@".\nod"))
                        Directory.CreateDirectory(@".\nod");
                    client.DownloadFile(NOD_EXE_URL, NOD_PATH);
                    client.DownloadFile(NOD_LICENSE_URL, @".\nod\LICENSE");
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

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
