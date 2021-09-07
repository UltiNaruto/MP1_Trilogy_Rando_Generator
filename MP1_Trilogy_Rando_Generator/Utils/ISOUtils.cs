using Nanook.NKit;
using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class ISOUtils
    {
        internal class NKIT
        {
            static bool IsValidFilename(string value)
            {
                return Regex.IsMatch(value, @"^[\w\-. ]+$");
            }

            public static bool ExtractISO(string filename)
            {
                bool result = true;
                if (!Directory.Exists(@".\tmp\nkit"))
                    Directory.CreateDirectory(@".\tmp\nkit");
                try
                {
                    using (var ndisc = new NDisc(null, File.OpenRead(filename)))
                    {
                        if (ndisc == null)
                            throw new System.Exception();
                        if (ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3ME01" &&
                            ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3MP01" &&
                            ndisc.ExtractBasicInfo().Id.Substring(0, 6) != "R3IJ01")
                            throw new System.Exception();
                        ndisc.ExtractFiles(ext_f => true,
                        (f, ext_f) =>
                        {
                            var path = @".\tmp\nkit\DATA\" + (ext_f.PartitionId == null ? @"sys" : @"files");
                            if (ext_f.Path != "")
                                path += @"\" + ext_f.Path;
                            if (!Directory.Exists(path))
                                Directory.CreateDirectory(path);
                            using (var stream = File.OpenWrite(path + @"\" + ext_f.Name))
                                f.Copy(stream, ext_f.Length);
                        });
                    }
                }
                catch
                {
                    result = false;
                }
                if (Directory.Exists(@".\Dats"))
                    Directory.Delete(@".\Dats", true);
                if (Directory.Exists(@".\Processed"))
                    Directory.Delete(@".\Processed", true);
                if (Directory.Exists(@".\Recovery"))
                    Directory.Delete(@".\Recovery", true);
                if (!result)
                    Directory.Delete(@".\tmp\nkit", true);
                return result;
            }
        }

        internal class NOD
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
                    ProcessStartInfo info = new ProcessStartInfo(NOD_PATH, "extract -f \"" + filename + "\" .\\tmp\\" + (isGC_ISO ? "gc" : "wii"));
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
                        if (title_id.StartsWith("R3M"))
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

                    ProcessStartInfo info = new ProcessStartInfo(NOD_PATH, (isGC_ISO ? "makegcn .\\tmp\\gc" : "makewii .\\tmp\\wii") + " \"" + filename + "\"");
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

        internal class WIT
        {
            private const string WIT_PATH = @".\wit\bin\wit.exe";
            private const string WIT_URL = "https://wit.wiimm.de/download/wit-v3.04a-r8427-cygwin32.zip";

            public static bool Installed()
            {
                return File.Exists(WIT_PATH);
            }

            public static bool Init()
            {
                try
                {
                    if (Installed())
                        return true;
                    using (var client = new WebClientPlus())
                    {
                        client.DownloadFile(WIT_URL, @".\tmp\wit.zip");
                        ZipFile.ExtractToDirectory(@".\tmp\wit.zip", @".");
                        Directory.Move(@".\wit-v3.04a-r8427-cygwin32", @".\wit");
                        File.Delete(@".\tmp\wit.zip");
                        return true;
                    }
                }
                catch
                {
                    return false;
                }
            }

            public static bool CreateCompressISO(string filename, bool isGC_ISO, String GameCode)
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, "COPY -d \"" + filename + "\" -s .\\tmp\\wii -C  --id " + GameCode);
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

            public static bool CreateWBFS(string filename, String GameCode)
            {
                try
                {
                    ProcessStartInfo info = new ProcessStartInfo(WIT_PATH, "COPY -s \".\\tmp\\wii\" -d \"" + filename + "\" -B --id \"" + GameCode + "\"");
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
}
