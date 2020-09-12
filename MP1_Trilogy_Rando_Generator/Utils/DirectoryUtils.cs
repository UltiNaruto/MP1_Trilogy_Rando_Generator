using System;
using System.IO;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    class DirectoryUtils
    {
        public static void Copy(String src, String dst, bool recursive=false)
        {
            var _dir = default(String);
            String f_src = Path.GetFullPath(src);
            String f_dst = Path.GetFullPath(dst);
            if (!Directory.Exists(f_dst))
                Directory.CreateDirectory(f_dst);
            if (recursive)
            {
                foreach (var dir in Directory.EnumerateDirectories(f_src))
                {
                    _dir = Path.GetFileNameWithoutExtension(dir);
                    Copy(f_src + Path.DirectorySeparatorChar + _dir, f_dst + Path.DirectorySeparatorChar + _dir, recursive);
                }
            }
            foreach (var file in Directory.EnumerateFiles(f_src))
            {
                File.Copy(file, file.Replace(f_src, f_dst));
            }
        }
    }
}
