using System;
using System.Diagnostics;
using System.IO;

namespace MP1_Trilogy_Rando_Generator
{
    class RandomizerManager
    {
        public static bool Run(String path)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(path);
                info.WorkingDirectory = Directory.GetCurrentDirectory();
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