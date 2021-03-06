﻿using System;
using System.Diagnostics;
using System.IO;
using System.Management;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    internal class ProcessUtils
    {
        internal static void KillChildrenProcesses(int id)
        {
            var searcher = default(ManagementObjectSearcher);
            var collection = default(ManagementObjectCollection);
            var childProcessId = default(int);

            searcher = new ManagementObjectSearcher("SELECT * FROM Win32_Process WHERE ParentProcessId=" + Process.GetCurrentProcess().Id);
            collection = searcher.Get();
            if (collection.Count > 0)
            {
                foreach (var item in collection)
                {
                    childProcessId = (int)item["ProcessId"];
                    if (childProcessId != id)
                    {
                        KillChildrenProcesses(childProcessId);
                        Process.GetProcessById(childProcessId).Kill();
                    }
                }
            }
        }

        public static bool Run(String path, params String[] args)
        {
            try
            {
                ProcessStartInfo info = new ProcessStartInfo(path);
                info.WorkingDirectory = Directory.GetCurrentDirectory();
                info.Arguments = String.Join(" ", args);
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