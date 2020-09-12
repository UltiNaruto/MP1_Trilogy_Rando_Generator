using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class FileUtils
    {
        internal static void NullifyFiles(String path, params String[] excludedFiles)
        {
            NullifyFiles(path, "*.*", false, excludedFiles);
        }

        internal static void NullifyFiles(String path, bool recursively, params String[] excludedFiles)
        {
            NullifyFiles(path, "*.*", recursively, excludedFiles);
        }

        internal static void NullifyFiles(String path, String filter, params String[] excludedFiles)
        {
            NullifyFiles(path, filter, false, excludedFiles);
        }

        internal static void NullifyFiles(String path, String filter, bool recursively, params String[] excludedFiles)
        {
            foreach (var file in Directory.EnumerateFiles(path, filter, recursively ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
                if(!excludedFiles.Contains(Path.GetFileName(file)))
                    File.WriteAllText(file, "");
        }
    }
}
