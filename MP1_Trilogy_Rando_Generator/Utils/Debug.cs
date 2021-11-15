using System;
using System.IO;
using System.Windows.Forms;

namespace MP1_Trilogy_Rando_Generator.Utils
{
    public static class Debug
    {
        static readonly String LogPath = String.Join(""+Path.DirectorySeparatorChar, Path.GetDirectoryName(Application.ExecutablePath), "logs", $"{DateTime.Now.Day:00}_{DateTime.Now.Month:00}_{DateTime.Now.Year:0000}.log");

        public static void Log(String text = "")
        {
            String parentDir = Path.GetDirectoryName(LogPath);
            if (!Directory.Exists(parentDir))
                Directory.CreateDirectory(parentDir);
            using (var stream = new StreamWriter(File.Open(LogPath, FileMode.OpenOrCreate, FileAccess.Write)))
            {
                stream.BaseStream.Position = stream.BaseStream.Length;
                stream.WriteLine(text);
            }
        }

        public static void LogException(Exception ex)
        {
            Log(ex.Message);
            Log(ex.StackTrace);
            Log();
        }
    }
}
